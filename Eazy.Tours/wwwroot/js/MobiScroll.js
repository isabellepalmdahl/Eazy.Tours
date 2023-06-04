mobiscroll.setOptions({
    theme: 'ios',
    themeVariant: 'light'
});

$(function () {
    var min = '2022-05-27T00:00';
    var max = '2022-11-27T00:00';


    $('#demo-booking-datetime').mobiscroll().datepicker({
        display: 'inline',
        controls: ['calendar', 'timegrid'],
        min: min,
        max: max,
        minTime: '08:00',
        maxTime: '19:59',
        stepMinute: 180,
        width: null,
        onPageLoading: function (event, inst) {
            getDatetimes(event.firstDay, function callback(bookings) {
                inst.setOptions({
                    labels: bookings.labels,
                    invalid: bookings.invalid
                });
            });
        }
    });


    function getPrices(d, callback) {
        var invalid = [],
            labels = [];

        mobiscroll.util.http.getJson('https://trial.mobiscroll.com/getprices/?year=' + d.getFullYear() + '&month=' + d.getMonth(), function (bookings) {
            for (var i = 0; i < bookings.length; ++i) {
                var booking = bookings[i],
                    d = new Date(booking.d);

                if (booking.price > 0) {
                    labels.push({
                        start: d,
                        title: '$' + booking.price,
                        textColor: '#e1528f'
                    });
                } else {
                    invalid.push(d);
                }
            }
            callback({ labels: labels, invalid: invalid });
        }, 'jsonp');
    }

    function getDatetimes(day, callback) {
        var invalid = [];
        var labels = [];

        mobiscroll.util.http.getJson('https://trial.mobiscroll.com/getbookingtime/?year=' + day.getFullYear() + '&month=' + day.getMonth(), function (bookings) {
            for (var i = 0; i < bookings.length; ++i) {
                var booking = bookings[i];
                var bDate = new Date(booking.d);

                if (booking.nr > 0) {
                    labels.push({
                        start: bDate,
                        title: booking.nr + ' SPOTS',
                        textColor: '#e1528f'
                    });
                    $.merge(invalid, booking.invalid);
                } else {
                    invalid.push(bDate);
                }
            }
            callback({ labels: labels, invalid: invalid });
        }, 'jsonp');
    }

    function getBookings(d, callback) {
        var invalid = [],
            labels = [];

        mobiscroll.util.http.getJson('https://trial.mobiscroll.com/getbookings/?year=' + d.getFullYear() + '&month=' + d.getMonth(), function (bookings) {
            for (var i = 0; i < bookings.length; ++i) {
                var booking = bookings[i],
                    d = new Date(booking.d);

                if (booking.nr > 0) {
                    labels.push({
                        start: d,
                        title: booking.nr + ' SPOTS',
                        textColor: '#e1528f'
                    });
                } else {
                    invalid.push(d);
                }
            }
            callback({ labels: labels, invalid: invalid });
        }, 'jsonp');
    }
});