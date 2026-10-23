// Run with: node tests/rma-order-generation.test.cjs
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');

function run() {
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
        .match(/<a[^>]*id="btncreateorder"[\s\S]*?<\/a>/)[0];

    for (const permission of [false, undefined, null]) {
        for (const ready of [false, true]) {
            const html = button({ canGenerateOrderForCurrentUser: permission, canGenerateOrder: ready });
            assert.match(html, /title="awaiting order generation"/);
            assert.match(html, /fa-hourglass-half/);
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
        const generated = button({ canGenerateOrderForCurrentUser: permission, orderNumber: '98765' });
        assert.match(generated, /Order #98765 Generated/);
        assert.doesNotMatch(generated, /OnCreateOrderClicked/);
    }
    return 'RMA order buttons: authorized generation, inactive placeholder, product readiness, and existing orders passed';
}

module.exports = run;
if (require.main === module) console.log(run());
