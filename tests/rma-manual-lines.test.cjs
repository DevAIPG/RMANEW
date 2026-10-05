// Run with: node tests/rma-manual-lines.test.cjs
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');

function run() {
    const handlers = {};
    const source = fs.readFileSync(path.join(__dirname, '../Amphenol.RMA/wwwroot/js/RmaRequest.js'), 'utf8');
    function collection(elements) {
        return {
            length: elements.length,
            prop(name, value) {
                if (value === undefined) return elements[0]?.[name];
                elements.forEach(x => { x[name] = value; }); return this;
            },
            val(value) {
                if (value === undefined) return elements[0]?.value;
                elements.forEach(x => { x.value = value; }); return this;
            },
            toggleClass(name, enabled) {
                elements.forEach(x => enabled ? x.classes.add(name) : x.classes.delete(name)); return this;
            },
            addClass(names) {
                names.split(' ').forEach(name => this.toggleClass(name, true)); return this;
            },
            removeClass(names) {
                names.split(' ').forEach(name => this.toggleClass(name, false)); return this;
            },
            attr(name, value) { elements.forEach(x => { x[name] = value; }); return this; },
            each(callback) { elements.forEach(x => callback.call(x)); return this; },
            rules(operation, rules) {
                elements.forEach(x => {
                    if (!x.ownerRow.validatorInitialized) throw new Error("Form validator is not initialized");
                    x.rules = rules;
                }); return this;
            },
            closest() { return elements[0].ownerRow; },
            empty() { return this; }, trigger() { return this; }, ready() { return this; },
            on(event, selector, callback) { handlers[selector] = callback; return this; }
        };
    }
    function row(manual) {
        const fields = {};
        for (const name of ['no-invoice-input', 'invoice-input', 'sequence-input', 'partnumber-input',
            'quantity-input', 'price-input', 'unitcost-input', 'invoice-selection', 'sequence-selection',
            'no-invoice-placeholder', 'manual-line-help', 'btn-search-partnumber', 'manual-part-error']) {
            fields[name] = { value: '', checked: false, classes: new Set() };
        }
        fields['no-invoice-input'].checked = manual;
        const result = {
            fields, length: 1, validatorInitialized: false,
            validate() { this.validatorInitialized = true; },
            find(selector) { return collection(selector.split(',').map(x => fields[x.trim().replace(/^\./, '')]).filter(Boolean)); }
        };
        Object.values(fields).forEach(field => { field.ownerRow = result; });
        return result;
    }
    const $ = value => value && value.ownerRow ? collection([value]) : collection([]);
    let request;
    $.ajax = options => { request = options; };
    let priceWarnings = 0;
    const context = vm.createContext({ $, document: {}, DataTransfer: class {}, console, Swal: { fire: () => { priceWarnings++; } } });
    vm.runInContext(source, context);
    context.CalculateTotalRmaValue = () => {};

    const manual = row(true);
    manual.fields['invoice-input'].value = '0';
    manual.fields['sequence-input'].value = '0';
    manual.fields['partnumber-input'].value = 'PART001';
    context.ApplyInvoiceMode(manual, false);
    assert.equal(manual.fields['partnumber-input'].value, 'PART001');
    assert.equal(manual.fields['partnumber-input'].readonly, true);
    assert.equal(manual.fields['partnumber-input'].placeholder, 'Select an ERP part');
    assert.equal(manual.fields['invoice-input'].disabled, true);
    assert.ok(manual.fields['sequence-selection'].classes.has('d-none'));
    assert.ok(!manual.fields['btn-search-partnumber'].classes.has('d-none'));

    const noValidator = row(true);
    noValidator.validate = undefined;
    noValidator.fields['invoice-input'].value = '123456';
    noValidator.fields['sequence-input'].value = '3';
    context.ApplyInvoiceMode(noValidator, true);
    assert.equal(noValidator.fields['invoice-input'].value, '');
    assert.equal(noValidator.fields['sequence-input'].value, '');
    assert.equal(noValidator.fields['invoice-input'].disabled, true);
    assert.ok(noValidator.fields['invoice-selection'].classes.has('d-none'));
    assert.ok(noValidator.fields['sequence-selection'].classes.has('d-none'));

    const switched = row(true);
    for (const field of ['invoice-input', 'sequence-input', 'partnumber-input', 'quantity-input', 'price-input', 'unitcost-input'])
        switched.fields[field].value = '123';
    context.ApplyInvoiceMode(switched, true);
    for (const field of ['invoice-input', 'sequence-input', 'partnumber-input', 'quantity-input', 'price-input', 'unitcost-input'])
        assert.equal(switched.fields[field].value, '');

    switched.fields['no-invoice-input'].checked = false;
    context.ApplyInvoiceMode(switched, true);
    assert.equal(switched.fields['invoice-input'].disabled, false);
    assert.equal(switched.fields['partnumber-input'].readonly, true);
    assert.ok(switched.fields['btn-search-partnumber'].classes.has('d-none'));

    vm.runInContext("knownRmaParts = new Set(['PART001']);", context);
    manual.fields['partnumber-input'].value = 'UNKNOWN';
    assert.equal(context.ValidateManualPart(manual), false);
    assert.ok(manual.fields['partnumber-input'].classes.has('border-danger'));
    manual.fields['partnumber-input'].value = ' part001 ';
    assert.equal(context.ValidateManualPart(manual), true);
    assert.ok(manual.fields['manual-part-error'].classes.has('d-none'));

    const invoiceLine = row(false);
    invoiceLine.fields['invoice-input'].value = '123456';
    invoiceLine.fields['sequence-input'].value = '3';
    context.testRow = invoiceLine;
    vm.runInContext('selectedLineRow = testRow;', context);
    context.SetValuesBasedOnInvoice('123456');
    context.testRow = manual;
    vm.runInContext('selectedLineRow = testRow;', context);
    request.success([{ price: 10, std: 5, loc: 'MRM' }]);
    assert.equal(invoiceLine.fields['price-input'].value, '10.00');
    assert.equal(manual.fields['price-input'].value, '');
    invoiceLine.fields['no-invoice-input'].checked = true;
    invoiceLine.fields['price-input'].value = '20';
    request.success([{ price: 10, std: 5, loc: 'MRM' }]);
    assert.equal(invoiceLine.fields['price-input'].value, '20');
    assert.equal(manual.fields['price-input'].min, 0);
    assert.equal(switched.fields['price-input'].min, 0.01);
    assert.equal(manual.fields['unitcost-input'].rules.min, 0);
    manual.fields['price-input'].value = '0';
    manual.fields['unitcost-input'].value = '0';
    handlers['.price-input, .unitcost-input'].call(manual.fields['price-input']);
    assert.equal(priceWarnings, 0);
    assert.ok(!manual.fields['price-input'].classes.has('border-danger'));
    switched.fields['price-input'].value = '0';
    switched.fields['unitcost-input'].value = '0';
    handlers['.price-input, .unitcost-input'].call(switched.fields['price-input']);
    assert.equal(priceWarnings, 1);
    assert.ok(switched.fields['price-input'].classes.has('border-danger'));
    return 'Passed 7 manual RMA line browser scenarios';
}

module.exports = run;
if (require.main === module) console.log(run());
