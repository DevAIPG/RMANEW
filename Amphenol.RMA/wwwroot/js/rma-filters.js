(function ($) {
    'use strict';

    var table;
    var filters = {};
    var form = document.getElementById('rmaFilterForm');
    var from = document.getElementById('rmaDateFrom');
    var to = document.getElementById('rmaDateTo');

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
            (!filters.customer || text(row.customerName) === filters.customer) &&
            (!filters.approver || text(row.approver) === filters.approver) &&
            (!filters.type || text(row.rmatypeofrequest) === filters.type);
    }

    function populateOptions(id, property, rows) {
        var select = document.getElementById(id);
        var selected = select.value;
        var values = rows.map(function (row) { return text(row[property]); }).filter(Boolean);
        // Keep an applied selection when refreshed data no longer contains it.
        if (selected) values.push(selected);
        select.length = 1;
        Array.from(new Set(values)).sort(function (a, b) { return a.localeCompare(b); }).forEach(function (value) {
            select.add(new Option(value, value));
        });
        select.value = selected;
    }

    $('#GeneratedRmaTable').on('init.dt.rmaFilters', function (event, settings) {
        if (settings.nTable !== this) return;
        table = new $.fn.dataTable.Api(settings);
        $('#rmaFilterToggle').insertAfter(table.buttons().container());
        var rows = table.rows().data().toArray();
        populateOptions('rmaCustomerName', 'customerName', rows);
        populateOptions('rmaApprover', 'approver', rows);
        populateOptions('rmaRequestType', 'rmatypeofrequest', rows);
        table.search.fixed('rmaFilters', matchesRow);
        table.draw();
    });

    $('#GeneratedRmaTable').on('destroy.dt.rmaFilters', function (event, settings) {
        if (settings.nTable !== this) return;
        // Preserve the button and its handler when the table is rebuilt.
        $('#rmaFilterToggle').appendTo('#rmaFilterButtonHost');
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
            part: text($('#rmaPartNumber').val()).toLowerCase(),
            customer: $('#rmaCustomerName').val(),
            approver: $('#rmaApprover').val(),
            type: $('#rmaRequestType').val()
        };
        var count = Number(!!(filters.from || filters.to)) + Number(!!filters.part) +
            Number(!!filters.customer) + Number(!!filters.approver) + Number(!!filters.type);
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
        to.setCustomValidity('');
        applyFilters();
    });
})(jQuery);
