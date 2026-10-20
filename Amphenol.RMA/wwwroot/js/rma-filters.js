(function ($) {
    'use strict';

    var table;
    var filters = {};
    var form = document.getElementById('rmaFilterForm');
    var from = document.getElementById('rmaDateFrom');
    var to = document.getElementById('rmaDateTo');
    var valueFilters = [
        { id: 'rmaCustomerName', key: 'customer', property: 'customerName' },
        { id: 'rmaApprover', key: 'approver', property: 'approver' },
        { id: 'rmaRequestType', key: 'type', property: 'rmatypeofrequest' },
        { id: 'rmaApprovalStatus', key: 'status', property: 'status' },
        { id: 'rmaBuiltLocation', key: 'location', property: 'wherebuilt' }
    ];
    var openFilter;

    function closeMenu(restoreFocus) {
        if (!openFilter) return;
        var current = openFilter;
        current.menu.hidden = true;
        current.button.setAttribute('aria-expanded', 'false');
        openFilter = null;
        if (restoreFocus) current.button.focus();
    }

    function updateSummary(filter) {
        var count = filter.values.filter(function (value) { return filter.excluded.has(value); }).length;
        filter.button.textContent = count === 0 ? filter.allLabel :
            count === filter.values.length ? 'None selected' : count + ' excluded';
    }

    function renderChoices(filter) {
        var query = filter.search.value.trim().toLowerCase();
        var visible = filter.values.filter(function (value) {
            return (value || '(Blanks)').toLowerCase().includes(query);
        });
        filter.list.replaceChildren();
        var checkedCount = visible.filter(function (value) { return !filter.excluded.has(value); }).length;
        filter.all.checked = visible.length > 0 && checkedCount === visible.length;
        filter.all.indeterminate = checkedCount > 0 && checkedCount < visible.length;
        filter.all.disabled = visible.length === 0;
        filter.allText.textContent = query ? 'Select all matches' : 'Select all';
        visible.forEach(function (value) {
            var label = document.createElement('label');
            var checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.checked = !filter.excluded.has(value);
            checkbox.addEventListener('change', function () {
                if (checkbox.checked) filter.excluded.delete(value);
                else filter.excluded.add(value);
                updateSummary(filter);
                var boxes = Array.from(filter.list.querySelectorAll('input'));
                var selected = boxes.filter(function (box) { return box.checked; }).length;
                filter.all.checked = selected === boxes.length;
                filter.all.indeterminate = selected > 0 && selected < boxes.length;
            });
            label.append(checkbox, document.createTextNode(value || '(Blanks)'));
            filter.list.append(label);
        });
        if (!visible.length) filter.list.textContent = 'No matching values';
    }

    valueFilters.forEach(function (filter) {
        var select = document.getElementById(filter.id);
        filter.allLabel = select.options[0].textContent;
        filter.values = [];
        filter.excluded = new Set();
        filter.button = document.createElement('button');
        filter.button.type = 'button';
        filter.button.id = filter.id;
        filter.button.className = 'form-control form-control-sm rma-value-toggle';
        filter.button.setAttribute('aria-expanded', 'false');
        filter.button.setAttribute('aria-controls', filter.id + 'Menu');
        var label = select.parentElement.querySelector('label');
        label.id = filter.id + 'Label';
        filter.button.setAttribute('aria-labelledby', label.id + ' ' + filter.id);
        select.replaceWith(filter.button);
        filter.menu = document.createElement('div');
        filter.menu.id = filter.id + 'Menu';
        filter.menu.className = 'rma-value-menu';
        filter.menu.hidden = true;
        filter.menu.setAttribute('role', 'group');
        filter.menu.setAttribute('aria-labelledby', label.id);
        filter.search = document.createElement('input');
        filter.search.type = 'search';
        filter.search.className = 'form-control form-control-sm mb-2';
        filter.search.placeholder = 'Search values';
        filter.search.setAttribute('aria-label', 'Search ' + label.textContent);
        var allLabel = document.createElement('label');
        filter.all = document.createElement('input');
        filter.all.type = 'checkbox';
        filter.allText = document.createElement('span');
        allLabel.append(filter.all, filter.allText);
        filter.list = document.createElement('div');
        filter.list.className = 'rma-value-list';
        var hint = document.createElement('small');
        hint.className = 'text-muted d-block mt-2';
        hint.textContent = 'Uncheck values to exclude. Use Apply filters when ready.';
        var done = document.createElement('button');
        done.type = 'button';
        done.className = 'btn btn-outline-secondary btn-sm mt-2';
        done.textContent = 'Done';
        done.addEventListener('click', function () { closeMenu(true); });
        filter.menu.append(filter.search, allLabel, filter.list, hint, done);
        document.body.append(filter.menu);
        filter.search.addEventListener('input', function () { renderChoices(filter); });
        filter.all.addEventListener('change', function () {
            var query = filter.search.value.trim().toLowerCase();
            filter.values.forEach(function (value) {
                if (!(value || '(Blanks)').toLowerCase().includes(query)) return;
                if (filter.all.checked) filter.excluded.delete(value);
                else filter.excluded.add(value);
            });
            renderChoices(filter);
            updateSummary(filter);
        });
        filter.button.addEventListener('click', function () {
            var wasOpen = openFilter === filter;
            closeMenu(false);
            if (wasOpen) return;
            filter.search.value = '';
            renderChoices(filter);
            var rect = filter.button.getBoundingClientRect();
            var width = Math.min(300, window.innerWidth - 16);
            filter.menu.style.width = width + 'px';
            filter.menu.style.left = Math.max(8, Math.min(rect.left, window.innerWidth - width - 8)) + 'px';
            var below = window.innerHeight - rect.bottom - 8;
            var above = rect.top - 8;
            var useAbove = below < 260 && above > below;
            filter.menu.style.top = useAbove ? 'auto' : rect.bottom + 4 + 'px';
            filter.menu.style.bottom = useAbove ? window.innerHeight - rect.top + 4 + 'px' : 'auto';
            filter.menu.style.maxHeight = Math.max(80, useAbove ? above : below) + 'px';
            filter.menu.hidden = false;
            filter.button.setAttribute('aria-expanded', 'true');
            openFilter = filter;
            filter.search.focus();
        });
        updateSummary(filter);
    });

    document.addEventListener('click', function (event) {
        if (openFilter && !openFilter.menu.contains(event.target) && !openFilter.button.contains(event.target)) closeMenu(false);
    });
    document.addEventListener('keydown', function (event) {
        if (openFilter && event.key === 'Escape') {
            event.preventDefault();
            closeMenu(true);
        }
    });
    document.addEventListener('focusin', function (event) {
        if (openFilter && !openFilter.menu.contains(event.target) && event.target !== openFilter.button) closeMenu(false);
    });
    window.addEventListener('resize', function () { closeMenu(false); });
    window.addEventListener('scroll', function (event) {
        if (openFilter && !openFilter.menu.contains(event.target)) closeMenu(false);
    }, true);

    function text(value) {
        return value == null ? '' : String(value).trim();
    }

    // Compare calendar dates without timezone conversion. Existing dates can be
    // ISO (from the date input) or US month/day/year (from legacy records).
    function dateKey(value) {
        var input = text(value);
        var match = /^(\d{4})-(\d{2})-(\d{2})(?:$|[T\s])/.exec(input);
        if (!match) {
            var us = /^(\d{1,2})\/(\d{1,2})\/(\d{4})(?:$|\s)/.exec(input);
            if (!us) return null;
            match = [us[0], us[3], us[1], us[2]];
        }
        var year = Number(match[1]), month = Number(match[2]), day = Number(match[3]);
        var date = new Date(year, month - 1, day);
        if (date.getFullYear() !== year || date.getMonth() !== month - 1 || date.getDate() !== day) return null;
        return year * 10000 + month * 100 + day;
    }

    function matchesRow(search, row) {
        if (filters.from || filters.to) {
            var date = dateKey(row.date);
            if (date === null || (filters.from && date < filters.from) || (filters.to && date > filters.to)) return false;
        }
        return (!filters.part || text(row.customerpartno).toLowerCase().includes(filters.part)) &&
            valueFilters.every(function (filter) {
                return !filters[filter.key] || !filters[filter.key].has(text(row[filter.property]));
            });
    }

    function populateOptions(filter, rows) {
        // Keep exclusions across refreshes, including values temporarily absent.
        var values = rows.map(function (row) { return text(row[filter.property]); });
        filter.values = Array.from(new Set(values.concat(Array.from(filter.excluded)))).sort(function (a, b) {
            return a.localeCompare(b);
        });
        renderChoices(filter);
        updateSummary(filter);
    }

    $('#GeneratedRmaTable').on('init.dt.rmaFilters', function (event, settings) {
        if (settings.nTable !== this) return;
        table = new $.fn.dataTable.Api(settings);
        $('#rmaFilterToggle').insertAfter(table.buttons().container());
        var rows = table.rows().data().toArray();
        valueFilters.forEach(function (filter) { populateOptions(filter, rows); });
        table.search.fixed('rmaFilters', matchesRow);
        table.draw();
    });

    $('#GeneratedRmaTable').on('destroy.dt.rmaFilters', function (event, settings) {
        if (settings.nTable !== this) return;
        // Preserve the button and its handler when the table is rebuilt.
        $('#rmaFilterToggle').appendTo('#rmaFilterButtonHost');
        closeMenu(false);
    });

    $('#rmaFilterToggle').on('click', function () {
        var panel = document.getElementById('rmaFilterPanel');
        panel.hidden = !panel.hidden;
        this.setAttribute('aria-expanded', String(!panel.hidden));
    });

    function validateRange() {
        to.setCustomValidity(from.value && to.value && from.value > to.value
            ? 'Date to must be on or after Date from.' : '');
    }
    $('#rmaDateFrom, #rmaDateTo').on('input change', validateRange);

    function applyFilters() {
        filters = {
            from: dateKey(from.value),
            to: dateKey(to.value),
            part: text($('#rmaPartNumber').val()).toLowerCase()
        };
        var count = Number(!!(filters.from || filters.to)) + Number(!!filters.part);
        valueFilters.forEach(function (filter) {
            filters[filter.key] = new Set(filter.excluded);
            if (filter.excluded.size) count++;
        });
        closeMenu(false);
        $('#rmaFilterCount').text(count).prop('hidden', count === 0);
        if (table) table.draw();
    }

    $(form).on('submit', function (event) {
        event.preventDefault();
        validateRange();
        if (form.reportValidity()) applyFilters();
    });

    $('#rmaClearFilters').on('click', function () {
        form.reset();
        valueFilters.forEach(function (filter) {
            filter.excluded.clear();
            filter.search.value = '';
            renderChoices(filter);
            updateSummary(filter);
        });
        to.setCustomValidity('');
        applyFilters();
    });
})(jQuery);
