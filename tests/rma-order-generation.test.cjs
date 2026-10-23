// Run with: node tests/rma-order-generation.test.cjs
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');

async function run() {
    let options;
    const chain = {
        on() { return this; }, click() { return this; }, ready() { return this; },
        DataTable(value) { options = value; return this; }
    };
    const $ = () => chain;
    $.ajax = request => {
        if (request.url === '/Client/rma/GetAllrma') {
            request.success({ resid: '101', canCancelAnyRequest: false, data: [] });
        }
    };
    const context = { $, document: {}, window: { location: { pathname: '/Client/Rma', search: '' } } };
    vm.runInNewContext(fs.readFileSync(path.join(__dirname, '../Amphenol.RMA/wwwroot/js/Datatables_.js'), 'utf8'), context);
    context.cargarDatatableRma();
    const render = options.columns[0].render;
    const row = {
        id: 42, res_id: 101, status: 'Approved', sumbit: 'Submitted', canEdit: false,
        turno: '12345', canGenerateOrder: true, canGenerateOrderForCurrentUser: false
    };
    const button = changes => render(42, 'display', { ...row, ...changes })
        .match(/<button[^>]*id="btncreateorder"[\s\S]*?<\/button>/)[0];

    for (const permission of [false, undefined, null]) {
        for (const ready of [false, true]) {
            const html = button({ canGenerateOrderForCurrentUser: permission, canGenerateOrder: ready });
            assert.match(html, ready ? /title="Awaiting order generation"/ : /title="Awaiting returned products"/);
            assert.match(html, ready ? /fa-clock/ : /fa-truck/);
            assert.match(html, /aria-disabled="true"/);
            assert.doesNotMatch(html, /OnClick|OnCreateOrderClicked/i);
        }
    }
    const allowed = button({ canGenerateOrderForCurrentUser: true });
    assert.match(allowed, /OnCreateOrderClicked\(42\)/);
    assert.match(allowed, /title="Generate New Order"/);
    const awaitingProducts = button({ canGenerateOrderForCurrentUser: true, canGenerateOrder: false });
    assert.match(awaitingProducts, /Awaiting returned products/);
    assert.doesNotMatch(awaitingProducts, /OnCreateOrderClicked/);
    for (const permission of [false, true]) {
        for (const ready of [false, true]) {
            const generated = button({ canGenerateOrderForCurrentUser: permission, canGenerateOrder: ready, orderNumber: ' 98765 ' });
            assert.match(generated, /Order #\s*98765\s* Generated/);
            assert.match(generated, /btn-success/);
            assert.match(generated, /data-generated-order="98765"/);
            assert.doesNotMatch(generated, /aria-disabled/);
            assert.doesNotMatch(generated, /OnCreateOrderClicked/);
        }
    }

    const handlers = {};
    const elements = {
        generatedOrderNumber: { value: '', focus() {}, select() {}, setSelectionRange() {} },
        generatedOrderCopyStatus: { textContent: '' },
        GeneratedOrderModal: {}
    };
    let copiedText;
    let modalShown = false;
    let fallbackCopyResult = true;
    const modalContext = {
        $: () => ({
            ready() {},
            on(event, selector, callback) { handlers[selector] = callback; }
        }),
        document: {
            getElementById: id => elements[id],
            execCommand(command) { assert.equal(command, 'copy'); return fallbackCopyResult; }
        },
        navigator: { clipboard: { async writeText(text) { copiedText = text; } } },
        bootstrap: { Modal: { getOrCreateInstance(element) {
            assert.equal(element, elements.GeneratedOrderModal);
            return { show() { modalShown = true; } };
        } } }
    };
    vm.runInNewContext(fs.readFileSync(path.join(__dirname, '../Amphenol.RMA/wwwroot/js/rma.js'), 'utf8'), modalContext);
    const openOrder = order => handlers['#GeneratedRmaTable [data-generated-order]'].call({
        getAttribute: () => encodeURIComponent(order)
    });
    openOrder('98765');
    assert.equal(modalShown, true);
    assert.equal(elements.generatedOrderNumber.value, '98765');
    await handlers['#copyGeneratedOrderNumber']();
    assert.equal(copiedText, '98765');
    assert.equal(elements.generatedOrderCopyStatus.textContent, 'Order number copied.');
    openOrder('12345');
    assert.equal(elements.generatedOrderNumber.value, '12345');
    assert.equal(elements.generatedOrderCopyStatus.textContent, '');
    modalContext.navigator.clipboard = undefined;
    await handlers['#copyGeneratedOrderNumber']();
    assert.equal(elements.generatedOrderCopyStatus.textContent, 'Order number copied.');
    fallbackCopyResult = false;
    await handlers['#copyGeneratedOrderNumber']();
    assert.match(elements.generatedOrderCopyStatus.textContent, /Ctrl\+C/);
    return 'RMA order generation permissions, existing order modal, copy action, and clipboard fallback passed';
}

module.exports = run;
if (require.main === module) run().then(console.log);
