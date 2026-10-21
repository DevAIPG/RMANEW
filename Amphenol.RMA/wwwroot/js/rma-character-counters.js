(function ($) {
    'use strict';

    $(function () {
        $('#newRmaForm, #editRmaForm').each(function () {
            const form = $(this);
            form.find('[data-character-counter]').each(function () {
                const input = $(this);
                // Use MVC's validation metadata so the counter follows the model's SQL limit.
                const limit = Number(input.attr('data-val-length-max') || input.attr('maxlength'));
                if (!Number.isInteger(limit) || limit <= 0) return;

                input.attr('maxlength', limit);
                const counterId = this.id + '-character-count';
                const counter = $('<small>', {
                    id: counterId,
                    class: 'rma-character-count',
                    role: 'status',
                    'aria-live': 'polite',
                    'aria-atomic': 'true'
                });
                const describedBy = (input.attr('aria-describedby') || '').split(/\s+/).filter(Boolean);
                describedBy.push(counterId);
                input.attr('aria-describedby', describedBy.join(' '));

                // Input groups need the helper after their search button and validation message.
                if (input.parent().hasClass('input-group')) {
                    input.parent().append(counter);
                } else {
                    counter.insertAfter(input);
                    if (input.parent().hasClass('row')) {
                        counter.addClass('col-8 offset-4');
                    }
                }

                function update() {
                    const length = (input.val() || '').length;
                    let message = length + ' / ' + limit + ' characters';
                    if (length > limit) message += ' - ' + (length - limit) + ' over limit';
                    else if (length === limit) message += ' - Limit reached';
                    else if (length >= Math.ceil(limit * 0.9)) message += ' - Near limit';

                    counter.text(message)
                        .toggleClass('rma-character-count-warning', length >= Math.ceil(limit * 0.9) && length < limit)
                        .toggleClass('rma-character-count-limit', length >= limit);
                }

                // jQuery events also cover values populated by customer / ship-to lookups.
                input.on('input change', update);
                form.on('reset', function () { setTimeout(update, 0); });
                update();
            });
        });
    });
})(jQuery);
