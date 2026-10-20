// Run with: node tests/rma-filters.test.cjs
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');

function run() {
    const elements = {};
    const handlers = {};
    class Element {
        constructor(tag = 'div') {
            this.tag = tag;
            this.children = [];
            this.events = {};
            this.value = '';
            this.style = {};
        }
        set id(value) { this._id = value; elements[value] = this; }
        get id() { return this._id; }
        append(...children) { this.children.push(...children); }
        replaceChildren() { this.children = []; }
        replaceWith(element) { this.parentElement.children = [element]; }
        addEventListener(name, callback) { this.events[name] = callback; }
        setAttribute(name, value) { this[name] = value; }
        querySelector() { return this.label; }
        querySelectorAll(tag) {
            return this.children.flatMap(child => [
                ...(child.tag === tag ? [child] : []), ...child.querySelectorAll(tag)
            ]);
        }
        contains(target) { return this === target || this.children.some(child => child.contains(target)); }
        focus() { this.focused = true; }
        getBoundingClientRect() { return {left: 100, top: 100, bottom: 130}; }
        setCustomValidity(message) { this.validation = message; }
        reportValidity() { return !elements.rmaDateTo.validation; }
    }
    for (const id of ['rmaFilterForm', 'rmaDateFrom', 'rmaDateTo', 'rmaPartNumber',
        'GeneratedRmaTable', 'rmaFilterCount', 'rmaFilterToggle', 'rmaFilterPanel', 'rmaClearFilters',
        'rmaCustomerName', 'rmaApprover', 'rmaRequestType', 'rmaApprovalStatus', 'rmaBuiltLocation']) {
        const element = new Element();
        element.id = id;
        element.options = [{textContent: 'All values'}];
        element.parentElement = new Element();
        element.parentElement.label = new Element('label');
        element.parentElement.label.textContent = id;
    }
    elements.rmaFilterForm.reset = () => {
        for (const id of ['rmaDateFrom', 'rmaDateTo', 'rmaPartNumber']) elements[id].value = '';
    };
    const document = {
        body: new Element(), events: {},
        getElementById: id => elements[id],
        createElement: tag => new Element(tag),
        createTextNode: text => Object.assign(new Element('text'), {textContent: text}),
        addEventListener(name, callback) { this.events[name] = callback; }
    };
    let rows = [
        {status: 'Approved', wherebuilt: 'Nogales', customerName: 'A', date: '2026-10-20', customerpartno: 'ABC'},
        {status: 'Pending', wherebuilt: 'Nogales', customerName: 'B', date: '2026-10-19', customerpartno: 'DEF'},
        {status: 'Rejected', wherebuilt: 'Other', customerName: '', date: '2026-10-18', customerpartno: 'ABC'}
    ];
    let predicate;
    const api = {
        buttons: () => ({container() {}}),
        rows: () => ({data: () => ({toArray: () => rows})}),
        search: {fixed(name, callback) { predicate = callback; }}, draw() {}
    };
    function $(target) {
        const element = typeof target === 'string' ? elements[target.slice(1)] : target;
        return {
            on(event, callback) { handlers[(element?.id || target) + ':' + event] = callback; },
            val() { return element.value; },
            text(value) { element.textContent = value; return this; },
            prop(key, value) { element[key] = value; return this; },
            insertAfter() {}, appendTo() {}
        };
    }
    $.fn = {dataTable: {Api: function () { return api; }}};
    vm.runInNewContext(fs.readFileSync(path.join(__dirname, '../Amphenol.RMA/wwwroot/js/rma-filters.js'), 'utf8'), {
        jQuery: $, document, window: {innerWidth: 1400, innerHeight: 900, addEventListener() {}}
    });
    function refresh() {
        handlers['GeneratedRmaTable:init.dt.rmaFilters'].call(elements.GeneratedRmaTable, {}, {nTable: elements.GeneratedRmaTable});
    }
    function apply() { handlers['rmaFilterForm:submit']({preventDefault() {}}); }
    function clear() { handlers['rmaClearFilters:click'](); }
    function menu(id) { return elements[id + 'Menu']; }
    function search(id, query) {
        const input = menu(id).children[0];
        input.value = query;
        input.events.input();
    }
    function toggle(id, value, checked) {
        const list = menu(id).children[2];
        const label = list.children.find(label => label.children[1].textContent === value);
        const checkbox = label.children[0];
        checkbox.checked = checked;
        checkbox.events.change();
    }
    function selectAll(id, checked) {
        const checkbox = menu(id).children[1].children[0];
        checkbox.checked = checked;
        checkbox.events.change();
    }
    const matching = () => rows.filter(row => predicate('', row)).map(row => row.status);
    refresh();
    assert.deepEqual(matching(), ['Approved', 'Pending', 'Rejected']);
    toggle('rmaApprovalStatus', 'Rejected', false);
    assert.deepEqual(matching(), ['Approved', 'Pending', 'Rejected'], 'Changes wait for Apply');
    apply();
    assert.deepEqual(matching(), ['Approved', 'Pending']);
    assert.equal(elements.rmaApprovalStatus.textContent, '1 excluded');
    toggle('rmaBuiltLocation', 'Nogales', false);
    apply();
    assert.deepEqual(matching(), [], 'Exclusions combine across columns');
    assert.equal(elements.rmaFilterCount.textContent, 2);
    clear();
    selectAll('rmaApprovalStatus', false);
    apply();
    assert.deepEqual(matching(), [], 'Deselecting everything returns zero rows');
    assert.equal(elements.rmaApprovalStatus.textContent, 'None selected');
    selectAll('rmaApprovalStatus', true);
    search('rmaApprovalStatus', 'pend');
    selectAll('rmaApprovalStatus', false);
    apply();
    assert.deepEqual(matching(), ['Approved', 'Rejected'], 'Select all matches leaves hidden values alone');
    assert.equal(menu('rmaApprovalStatus').children[1].children[1].textContent, 'Select all matches');
    search('rmaApprovalStatus', 'no such status');
    assert.equal(menu('rmaApprovalStatus').children[1].children[0].disabled, true);
    refresh();
    assert.deepEqual(matching(), ['Approved', 'Rejected'], 'Refresh preserves applied exclusions');
    rows.push({status: 'Created', wherebuilt: 'New location'});
    refresh();
    assert.ok(matching().includes('Created'), 'New values are included unless explicitly excluded');
    clear();
    toggle('rmaCustomerName', '(Blanks)', false);
    apply();
    assert.deepEqual(matching(), ['Approved', 'Pending'], 'Blank values can be excluded');
    clear();
    elements.rmaPartNumber.value = 'abc';
    elements.rmaDateFrom.value = '2026-10-19';
    apply();
    assert.deepEqual(matching(), ['Approved'], 'Date and part filters still combine');
    clear();
    assert.equal(matching().length, rows.length);
    assert.equal(elements.rmaFilterCount.hidden, true);
    elements.rmaApprovalStatus.events.click();
    assert.equal(menu('rmaApprovalStatus').hidden, false);
    document.events.keydown({key: 'Escape', preventDefault() {}});
    assert.equal(menu('rmaApprovalStatus').hidden, true);
    assert.equal(elements.rmaApprovalStatus['aria-expanded'], 'false');
    assert.equal(elements.rmaApprovalStatus.focused, true);
    return 'RMA checkbox filters: exclusions, search, select all/none, blanks, refresh, clear, and Escape passed';
}

module.exports = run;
if (require.main === module) console.log(run());
