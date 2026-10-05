(function (window, document) {
    'use strict';

    var defaultMessage = 'An error occurred while processing your request. Check the request status before trying again. If the problem continues, contact support.';
    var dialog = document.getElementById('globalErrorModal');
    var ajaxAttached = false;

    function show(message, reference) {
        // Keep the first failure visible, including its reference, when several requests fail together.
        if (dialog.open) return;
        document.getElementById('globalErrorMessage').textContent = message || defaultMessage;
        document.getElementById('globalErrorReference').textContent = reference || '';
        document.getElementById('globalErrorReferenceRow').hidden = !reference;
        if (typeof dialog.showModal === 'function') {
            dialog.showModal();
        } else {
            window.alert((message || defaultMessage) + (reference ? '\nError reference: ' + reference : ''));
        }
    }

    function showResponse(status, problem) {
        // Only display details from our error contract; never display an HTML response or raw exception.
        if (problem && problem.title === 'Unable to complete the request' && problem.traceId) {
            show(problem.detail, problem.traceId);
        } else if (status === 401) {
            show('Your session has expired. Refresh the page and sign in again.');
        } else if (status === 403) {
            show('You do not have permission to complete this request.');
        } else if (status === 0) {
            show('Unable to reach the server. Check your connection and the request status before trying again.');
        } else {
            show(defaultMessage);
        }
    }

    function showRequestError(response, status) {
        if (status === 'abort' || (response && response.statusText === 'abort')) return;
        showResponse(response ? response.status : 0, response && response.responseJSON);
    }

    window.RmaErrors = {
        show: show,
        showRequestError: showRequestError,
        attachDataTablesHandler: function () {
            var table = window.jQuery && window.jQuery.fn && window.jQuery.fn.dataTable;
            if (!table || !table.ext) return;
            table.ext.errMode = function (settings, techNote, message) {
                if (window.console) window.console.error('DataTables error:', message);
                show('Unable to display table data. Refresh the page and try again.');
            };
        },
        attachAjaxHandler: function () {
            if (ajaxAttached || !window.jQuery) return;
            ajaxAttached = true;
            // A prefilter also covers requests that specify global: false.
            window.jQuery.ajaxPrefilter(function (options, originalOptions, xhr) {
                xhr.fail(showRequestError);
            });
        }
    };

    if (window.fetch) {
        var originalFetch = window.fetch;
        window.fetch = function () {
            return originalFetch.apply(window, arguments).then(function (response) {
                if (!response.ok && response.type !== 'opaque' && response.type !== 'opaqueredirect') {
                    var contentType = response.headers.get('content-type') || '';
                    if (contentType.indexOf('json') >= 0) {
                        return response.clone().json().catch(function () { return null; }).then(function (problem) {
                            showResponse(response.status, problem);
                            return response;
                        });
                    }
                    showResponse(response.status);
                }
                return response;
            }, function (error) {
                if (!error || error.name !== 'AbortError') showResponse(0);
                throw error;
            });
        };
    }

    window.addEventListener('error', function () { show(defaultMessage); });
    window.addEventListener('unhandledrejection', function (event) {
        if (!event.reason || event.reason.name !== 'AbortError') show(defaultMessage);
    });

    var initialError = document.querySelector('[data-global-error-message]');
    if (initialError) {
        show(initialError.getAttribute('data-global-error-message'), initialError.getAttribute('data-global-error-reference'));
    }
})(window, document);
