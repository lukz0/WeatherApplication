import {writable} from 'svelte/store';

const currentLanguage = writable('nb');

export {
    currentLanguage
};