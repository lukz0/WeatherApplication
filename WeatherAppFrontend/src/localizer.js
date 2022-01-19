import {get} from 'svelte/store';

const nb = {
    'North': 'Nord',
    'North-East': 'Nordøst',
    'East': 'Øst',
    'South-East': 'Sørøst',
    'South': 'Sør',
    'South-West': 'Sørvest',
    'West': 'West',
    'North-West': 'Nordvest',
    'Weather in ': 'Vær i ',
    'Precipitation: None': 'Nedbør: Ingen',
    'Precipitation': 'Nedbør',
    'day': 'dag',
    'Wind Velocity': 'Vindstyrke',
    'Wind From': 'Vind Fra',
    'Unknown Weather': 'Ukjent Vær',
    'Unknown Location': 'Ukjent Sted',
    'Unknown Direction': 'Ukjent Retning',
    'Last updated': 'Sist Oppdatert'
}

const localize = (english_str, currentLanguage) => {
    if (currentLanguage === 'nb') {
        let translated = nb[english_str];
        if (typeof translated === 'undefined') {
            return english_str;
        }
        return translated;
    }
    return english_str;
}

export default localize;