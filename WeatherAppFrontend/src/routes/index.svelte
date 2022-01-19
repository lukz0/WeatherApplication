<script context="module" lang="ts">
	/**
	 * @type {import('@sveltejs/kit').Load}
	 */
	export async function load(_session) {
		const url = 'http://127.0.0.1:3000/data.json';
		let res = await fetch(url);
		if (res.ok) {
			return {
				props: {
					data: await res.json()
				}
			};
		}
		return {
			status: res.status,
			error: new Error(`Could not load ${url}`)
		};
	}
</script>

<script lang="ts">
	import CurrentWeather from './_CurrentWeather.svelte';
	import UpcomingWeathersList from './_UpcomingWeathersList.svelte';
	import { currentLanguage as cl } from '../stores';

	const weatherIconPath = 'static/weathericons/';

	export let data = [];

	$: dataNotFirst = data.slice(1);

	function toggleLanguage() {
		if ($cl === 'nb') {
			cl.set('en');
			return;
		}
		cl.set('nb');
	}
</script>

<div id="svelte" class="container-sm">
	<CurrentWeather firstWeatherItem={data[0]} {weatherIconPath} />

	<div class="row weather-card-background pb-2 justify-content-end">
		<div class="col-auto">
			<button type="button" class="btn-sm" on:click={toggleLanguage}
				>{$cl === 'nb' ? 'View in English' : 'Vis på Norsk'}</button
			>
		</div>
	</div>

	<UpcomingWeathersList {dataNotFirst} {weatherIconPath} />
</div>

<style lang="scss">
	@import '../color';
</style>
