// Run with: node tests/global-errors.test.cjs
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');
const source = fs.readFileSync(path.join(__dirname, '../Amphenol.RMA/wwwroot/js/global-errors.js'), 'utf8');

function setup(fetch, initial) {
    const elements = {};
    for (const id of ['globalErrorModal', 'globalErrorMessage', 'globalErrorReference', 'globalErrorReferenceRow']) {
        elements[id] = { textContent: '', hidden: false };
    }
    const modal = elements.globalErrorModal;
    modal.showModal = () => { modal.open = true; };
    const listeners = {};
    let prefilter;
    const window = {
        fetch, addEventListener: (name, callback) => { listeners[name] = callback; },
        jQuery: { ajaxPrefilter: callback => { prefilter = callback; } }
    };
    const document = {
        getElementById: id => elements[id],
        querySelector: () => initial ? { getAttribute: name => initial[name] } : null
    };
    vm.runInNewContext(source, { window, document });
    window.RmaErrors.attachAjaxHandler();
    return { window, elements, listeners, fail: (xhr, status) => {
        let callback;
        prefilter({}, {}, { fail: handler => { callback = handler; } });
        callback(xhr, status);
    } };
}

async function run() {
    const problem = { title: 'Unable to complete the request', detail: 'Safe server message', traceId: 'ref-123' };
    const ajax = setup();
    ajax.fail({ status: 500, responseJSON: problem }, 'error');
    assert.equal(ajax.elements.globalErrorMessage.textContent, problem.detail);
    assert.equal(ajax.elements.globalErrorReference.textContent, 'ref-123');
    ajax.listeners.error({ message: 'Private exception' });
    assert.equal(ajax.elements.globalErrorReference.textContent, 'ref-123');

    const aborted = setup();
    aborted.fail({ status: 0, statusText: 'abort' }, 'abort');
    aborted.listeners.unhandledrejection({ reason: { name: 'AbortError' } });
    assert.ok(!aborted.elements.globalErrorModal.open);

    const raw = setup();
    raw.fail({ status: 500, responseText: '<script>private exception</script>' }, 'error');
    assert.ok(raw.elements.globalErrorModal.open);
    assert.ok(!raw.elements.globalErrorMessage.textContent.includes('private exception'));

    const response = { ok: false, status: 500, headers: { get: () => 'application/problem+json' },
        clone: () => ({ json: async () => problem }) };
    const fetched = setup(async () => response);
    assert.equal(await fetched.window.fetch('/approval'), response);
    assert.equal(fetched.elements.globalErrorReference.textContent, 'ref-123');

    const networkError = new Error('offline');
    const offline = setup(async () => { throw networkError; });
    await assert.rejects(offline.window.fetch('/approval'), error => error === networkError);
    assert.ok(offline.elements.globalErrorMessage.textContent.includes('connection'));

    const initial = setup(undefined, { 'data-global-error-message': 'Form failed', 'data-global-error-reference': 'form-ref' });
    assert.ok(initial.elements.globalErrorModal.open);
    assert.equal(initial.elements.globalErrorReference.textContent, 'form-ref');

    const scriptError = setup();
    scriptError.listeners.unhandledrejection({ reason: new Error('secret') });
    assert.ok(scriptError.elements.globalErrorModal.open);
    assert.ok(!scriptError.elements.globalErrorMessage.textContent.includes('secret'));
    return 'Passed 7 global error modal scenarios';
}

module.exports = run;
if (require.main === module) run().then(console.log).catch(error => { console.error(error); process.exitCode = 1; });
