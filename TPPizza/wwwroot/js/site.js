$.validator.addMethod('liste',
    function (value, element, params) {
        let nb = value.length;
        let min = params[1].min;
        let max = params[1].max;
        if (nb < min || nb > max) {
            return false;
        } else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.add('liste', ['min', 'max'], function (options) {
    let params = {
        min: parseInt(options.params['min']),
        max: parseInt(options.params['max'])
    }
    options.rules['liste'] = [null, params];
    options.messages['liste'] = options.message;
});