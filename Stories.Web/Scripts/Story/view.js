

$(document).on('click', '.ygstar',
    function (e) {
        let star = $(this);
        let val = star.data('value');
        let subject = star.data('subject');
        let story = star.data('story');

        unRate(subject);

        let rate = {
            StoryId: story,
            RateSubjectId: subject,
            Rate: val
        };

        $.ajax({
            url: '/Story/Rate',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(rate)
        }).done(function (result) {

            if (!result) {
                unRate(subject);
            }

        }).fail(function (xhr, text) {
            unRate(subject);
        })

        for (let i = 1; i <= parseInt(val); i++) {
            $(document).find(`.ygstar[data-value="${i}"][data-subject="${subject}"]`).addClass('checked');
        }
    })

function unRate(subject) {
    $(document).find(`.ygstar[data-subject="${subject}"]`).removeClass('checked');
}