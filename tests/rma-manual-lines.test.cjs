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
            length: elements.length, 0: elements[0],
            first() { return collection(elements.slice(0, 1)); },
            text(value) { if (value === undefined) return elements[0]?.text; elements.forEach(x => { x.text = value; }); return this; },
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
            attr(name, value) {
                if (value === undefined) return elements[0]?.[name];
                elements.forEach(x => { x[name] = value; }); return this;
            },
            each(callback) { elements.forEach(x => callback.call(x)); return this; },
            rules(operation, rules) {
                elements.forEach(x => {
                    if (!x.ownerRow.validatorInitialized) throw new Error("Form validator is not initialized");
                    x.rules = rules;
                }); return this;
            },
            valid() { elements.forEach(x => { x.validatedValue = x.value; }); return true; },
            closest() { return elements[0].ownerRow; },
            empty() { return this; }, trigger(event) { elements.forEach(x => { x.lastEvent = event; }); return this; }, ready() { return this; },
            on(event, selector, callback) { handlers[selector] = callback; handlers[event + ':' + selector] = callback; return this; }
        };
    }
    function row(manual) {
        const fields = {};
        for (const name of ['no-invoice-input', 'invoice-input', 'sequence-input', 'partnumber-input',
            'quantity-input', 'price-input', 'unitcost-input', 'invoice-selection', 'sequence-selection',
            'no-invoice-placeholder', 'manual-line-help', 'btn-search-partnumber', 'manual-part-error', 'price-amount-error', 'cost-amount-error', 'returncode-input', 'returncode-validation']) {
            fields[name] = { value: '', checked: false, classes: new Set() };
        }
        fields['no-invoice-input'].checked = manual;
        const result = {
            fields, length: 1, validatorInitialized: false,
            data(name, value) { if (value === undefined) return this[name]; this[name] = value; return this; },
            removeData(name) { delete this[name]; return this; },
            validate() { this.validatorInitialized = true; },
            next() { return this.messages; },
            remove() { this.removed = true; validationRows = validationRows.filter(row => row !== this); },
            find(selector) { return collection(selector.split(',').map(x => fields[x.trim().replace(/^\./, '')])
                .filter(field => field && !field.isMessage)); }
        };
        const messageNames = ['manual-part-error', 'price-amount-error', 'cost-amount-error', 'returncode-validation'];
        messageNames.forEach(name => { fields[name].isMessage = true; });
        result.messages = {
            removed: false,
            find(selector) { return collection(selector.split(',').map(x => fields[x.trim().replace(/^\./, '')])
                .filter(field => field && field.isMessage)); },
            remove() { this.removed = true; }
        };
        Object.values(fields).forEach(field => { field.ownerRow = result; });
        return result;
    }
    let validationForm = { length: 0 };
    let validationRows = [];
    const $ = value => {
        if (value === '#LineTable') return { closest: () => validationForm };
        if (value === '#LineTable tbody tr.rma-line-controls') return collection(validationRows);
        if (value && value.fields) return value;
        return value && value.ownerRow ? collection([value]) : collection([]);
    };
    let request;
    $.ajax = options => { request = options; };
    let priceWarnings = 0;
    const context = vm.createContext({ $, document: {}, DataTransfer: class {}, console, Swal: { fire: () => { priceWarnings++; } } });
    vm.runInContext(source, context);
    context.CalculateTotalRmaValue = () => {};

    const refreshedManual = row(true);
    const refreshedInvoice = row(false);
    validationRows = [refreshedManual, refreshedInvoice];
    let destroyed = 0, parsed = 0, removed = 0;
    validationForm = {
        length: 1,
        data: () => ({ destroy() { destroyed++; } }),
        removeData() { removed++; return this; }
    };
    // A missing adapter must not crash AJAX line addition or strip existing validation.
    context.RefreshLineValidation();
    assert.equal(removed, 0);
    assert.equal(refreshedManual.fields['invoice-input'].disabled, true);
    $.validator = {};
    context.RefreshLineValidation();
    assert.equal(removed, 0);
    $.validator.unobtrusive = { parse(form) { assert.equal(form, validationForm); parsed++; } };
    context.RefreshLineValidation();
    context.RefreshLineValidation();
    assert.equal(destroyed, 2, 'Dispose old handlers before attaching a new validator');
    assert.equal(parsed, 2);
    assert.equal(removed, 4);
    assert.equal(refreshedManual.fields['unitcost-input'].rules.min, 0);
    assert.equal(refreshedInvoice.fields['unitcost-input'].rules.min, 0.01);
    for (const page of ['Create', 'Edit']) {
        const markup = fs.readFileSync(path.join(__dirname,
            '../Amphenol.RMA/Areas/Client/Views/Rma/' + page + '.cshtml'), 'utf8');
        const adapter = markup.indexOf('jquery.validate.unobtrusive.min.js');
        assert.ok(adapter >= 0 && adapter < markup.indexOf('~/js/RmaRequest.js'),
            page + ' must load the adapter before the request script');
    }
    validationRows = [];
    validationForm = { length: 0 };
    delete $.validator;

    const manual = row(true);
    manual.fields['invoice-input'].value = '0';
    manual.fields['sequence-input'].value = '0';
    manual.fields['partnumber-input'].value = 'PART001';
    context.ApplyInvoiceMode(manual, false);
    assert.equal(manual.fields['partnumber-input'].value, 'PART001');
    assert.equal(manual.fields['partnumber-input'].readonly, true);
    assert.equal(manual.fields['partnumber-input'].placeholder, 'Select a part number');
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
    assert.equal(priceWarnings, 0);
    assert.ok(switched.fields['price-input'].classes.has('border-danger'));
    // Exercise the shipped validator rules: invoice lookup costs are not rounded.
    const validationSource = fs.readFileSync(path.join(__dirname,
        '../Amphenol.RMA/wwwroot/lib/jquery-validation/dist/jquery.validate.js'), 'utf8');
    function validatorMethod(name, nextName) {
        const start = validationSource.indexOf(name + ': function');
        const end = validationSource.indexOf(nextName + ': function', start);
        const expression = validationSource.slice(start + name.length + 2, end)
            .replace(/,\s*(?:\/\/[^\n]*\s*)*$/, '');
        return vm.runInContext('(' + expression + ')', context);
    }
    const stepRule = validatorMethod('step', 'equalTo');
    const minRule = validatorMethod('min', 'max');
    const normalizeRule = validatorMethod('normalizeAttributeRule', 'attributeRules');
    const validator = { optional: () => false };
    const view = fs.readFileSync(path.join(__dirname,
        '../Amphenol.RMA/Areas/Client/Views/Shared/RmaItemEditableLine.cshtml'), 'utf8');
    const secondLine = row(false);
    secondLine.fields['price-input'].value = '20.00';
    secondLine.fields['unitcost-input'].value = '5.123456';
    context.ApplyInvoiceMode(secondLine, false);
    // The previous four-place step rejected this otherwise valid invoice cost.
    secondLine.fields['unitcost-input'].type = 'number';
    assert.equal(stepRule.call(validator, '5.123456', secondLine.fields['unitcost-input'], 0.0001), false);
    function amountsValid(line) {
        return ['price-input', 'unitcost-input'].every(name => {
            const input = line.fields[name];
            const tag = view.match(new RegExp('<input[^>]*class="[^"]*' + name + '[^>]*>'))[0];
            const step = tag.match(/step="([^"]+)"/)[1];
            const rules = {};
            normalizeRule(rules, 'number', 'step', step);
            input.type = 'number';
            return minRule.call(validator, input.value, input, input.rules.min)
                && (rules.step === undefined || stepRule.call(validator, input.value, input, rules.step));
        });
    }
    assert.equal(amountsValid(manual), true, 'No invoice permits zero price and cost');
    assert.equal(amountsValid(secondLine), true, 'Saving a mixed request preserves invoice cost precision');
    assert.equal(amountsValid(switched), false, 'Invoice lines still reject zero amounts');
    manual.fields['unitcost-input'].value = '-1';
    assert.equal(amountsValid(manual), false, 'Manual lines still reject negative amounts');
    for (const field of ['InvoiceNumber', 'SequenceNumber', 'PartNumber', 'AuthorizedQuantity', 'Price', 'UnitCost']) {
        assert.ok(view.includes('data-valmsg-for="Lines[@(index)].' + field + '"'),
            field + ' errors must appear beside the indexed line input');
    }
    const paddedLine = row(false);
    const partInput = paddedLine.fields['partnumber-input'];
    partInput.value = '12345678901234567890     ';
    handlers['.partnumber-input'].call(partInput);
    assert.equal(partInput.value, '12345678901234567890');
    assert.equal(partInput.validatedValue, '12345678901234567890',
        'Normalize trailing ERP padding before invoking length validation');
    partInput.value = '123456789012345678901     ';
    handlers['.partnumber-input'].call(partInput);
    assert.equal(partInput.validatedValue.length, 21, 'Do not truncate genuine part numbers');
    partInput.value = ' PART 001   ';
    handlers['.partnumber-input'].call(partInput);
    assert.equal(partInput.validatedValue, ' PART 001', 'Preserve leading and embedded characters');
    const amountLine = row(false);
    amountLine.fields['price-input'].value = '5';
    amountLine.fields['unitcost-input'].value = '5';
    handlers['input:.price-input, .unitcost-input'].call(amountLine.fields['price-input']);
    assert.ok(!amountLine.fields['price-input'].classes.has('border-danger'), 'Do not interrupt initial typing');
    handlers['blur:.price-input, .unitcost-input'].call(amountLine.fields['unitcost-input']);
    assert.equal(amountLine.fields['price-amount-error'].text, 'Price must be greater than unit cost.');
    assert.equal(amountLine.fields['price-input']['aria-invalid'], 'true');
    amountLine.fields['price-input'].value = '6';
    handlers['input:.price-input, .unitcost-input'].call(amountLine.fields['price-input']);
    assert.ok(amountLine.fields['price-amount-error'].classes.has('d-none'));
    assert.equal(amountLine.fields['price-input']['aria-invalid'], 'false');
    amountLine.fields['price-input'].value = '0.014';
    amountLine.fields['unitcost-input'].value = '0.013';
    assert.equal(context.ValidateLineAmounts(amountLine), true, 'Compare original precision without rounding');
    manual.fields['price-input'].value = '0';
    manual.fields['unitcost-input'].value = '0';
    assert.equal(context.ValidateLineAmounts(manual), true);
    manual.fields['unitcost-input'].value = '-1';
    assert.equal(context.ValidateLineAmounts(manual), false);
    assert.ok(manual.fields['unitcost-input'].classes.has('border-danger'));
    manual.fields['unitcost-input'].value = '0';
    amountLine.fields['price-input'].value = '5';
    amountLine.fields['unitcost-input'].value = '5';
    amountLine.fields['invoice-input'].value = '123456';
    amountLine.fields['sequence-input'].value = '3';
    manual.fields['returncode-input'].value = '001';
    amountLine.fields['returncode-input'].value = '001';
    validationRows = [manual, amountLine];
    let blockedSave = false, scrolled = false;
    amountLine.fields['price-input'].scrollIntoView = () => { scrolled = true; };
    const saveForm = { fields: {}, find() {
        return collection(validationRows.flatMap(line => Object.values(line.fields))
            .filter(field => !field.disabled && field.classes.has('border-danger')));
    } };
    handlers['#newRmaForm, #editRmaForm'].call(saveForm, { preventDefault() { blockedSave = true; } });
    assert.equal(blockedSave, true, 'Save must validate even before either field has blurred');
    assert.equal(scrolled, true);
    assert.equal(amountLine.fields['price-input'].lastEvent, 'focus');
    assert.ok(!manual.fields['price-input'].classes.has('border-danger'), 'A valid zero manual line stays valid');
    amountLine.fields['price-input'].value = '6';
    blockedSave = false;
    handlers['#newRmaForm, #editRmaForm'].call(saveForm, { preventDefault() { blockedSave = true; } });
    assert.equal(blockedSave, false);
    amountLine.fields['returncode-input'].value = '   ';
    blockedSave = false;
    handlers['#newRmaForm, #editRmaForm'].call(saveForm, { preventDefault() { blockedSave = true; } });
    assert.equal(blockedSave, true);
    assert.equal(amountLine.fields['returncode-validation'].text, 'Select a return code.');
    assert.equal(amountLine.fields['returncode-input'].lastEvent, 'focus');
    assert.equal(amountLine.fields['returncode-input']['aria-invalid'], 'true');
    amountLine.fields['returncode-input'].value = '001';
    handlers['input change:.returncode-input'].call(amountLine.fields['returncode-input']);
    assert.equal(amountLine.fields['returncode-validation'].text, '');
    assert.equal(amountLine.fields['returncode-input']['aria-invalid'], 'false');
    blockedSave = false;
    handlers['#newRmaForm, #editRmaForm'].call(saveForm, { preventDefault() { blockedSave = true; } });
    assert.equal(blockedSave, false);
    assert.ok(view.includes('data-valmsg-for="Lines[@(index)].ReturnCode"'));
    assert.equal(priceWarnings, 0, 'Amount validation never opens a popup');
    context.ApplyInvoiceMode(amountLine, true);
    assert.equal(amountLine.data('amounts-validated'), undefined);
    assert.ok(amountLine.fields['price-amount-error'].classes.has('d-none'));
    validationRows = [];
    const controlMarkup = view.split('<tr class="rma-line-errors">')[0];
    assert.ok(!controlMarkup.includes('asp-validation-for='), 'Warnings must not change the controls row height');
    assert.ok(!controlMarkup.includes('price-amount-error'));
    assert.ok(view.includes('<tr class="rma-line-errors">'));
    assert.ok(view.includes('<td colspan="12">'));
    assert.equal(context.LineMessages(amountLine), amountLine.messages);
    assert.equal(amountLine.find('.returncode-validation').length, 0, 'Messages live outside the controls row');
    validationRows = [manual, amountLine];
    handlers['.btn-remove-rma-line'].call(amountLine.fields['price-input']);
    assert.equal(amountLine.removed, true);
    assert.equal(amountLine.messages.removed, true, 'Delete the associated warning row with the line');
    assert.equal(manual.messages.removed, false);
    assert.equal(validationRows.length, 1);
    return 'Manual RMA line regression checks passed, including separate warning rows';
}

module.exports = run;
if (require.main === module) console.log(run());
