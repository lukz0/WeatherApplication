<script lang="ts">
	import l from '../localizer';
	import { currentLanguage as cl } from '../stores';
	import weatherDescriptions from '../weatherDescriptions';
	import degreeRotationToDirections from '../degreeRotationToDirections';
	import timestampToString from '../timestampToString';

	export let dataNotFirst = [];
	export let weatherIconPath = '';

	function getWeatherDescription(weatherName: string, currentLanguage: string) {
		if (typeof weatherName !== 'string') weatherName = '';
		weatherName = weatherName.split('_')[0];

		let weatherDescription = weatherDescriptions[weatherName];
		if (!weatherDescription) {
			return l('Unknown Weather', currentLanguage);
		}
		let currentLang = $cl;
		return weatherDescription['desc_'.concat(currentLanguage)];
	}
</script>

{#each dataNotFirst as item, i}
	<div class="weather-card-background row rounded align-items-center mt-1 mb-1 p-2">
		<div class="col-auto">
			<img
				src="{weatherIconPath.concat(item.weatherType)}.png"
				alt={getWeatherDescription(item, $cl)}
				class="img-fluid"
				width="75"
				height="75"
			/>
		</div>
		<div class="col-sm">
			{#if typeof item?.date === 'string'}
				{timestampToString(new Date(item.date))}
			{:else}
				''
			{/if}
		</div>
		<div class="col-sm">
			{typeof item?.temperatureC === 'undefined' ? '???' : item.temperatureC} °C
		</div>
		<div class="col-sm">
			{getWeatherDescription(item?.weatherType, $cl)}
		</div>
		<div class="col-sm">
			{#if item.precipitationNext6Hours === 0}
				{l('Precipitation: None', $cl)}
			{:else if typeof item.precipitationNext6Hours === 'number'}
				{l('Precipitation', $cl)}: {(item.precipitationNext6Hours * 4).toFixed(2)}mm/{l('day', $cl)}
			{/if}
		</div>
		<div class="col-sm">
			{l('Wind Velocity', $cl)}: {item.windVelocity}m/s
		</div>
		<div class="col-sm">
			{l('Wind From', $cl)}: {l(degreeRotationToDirections(item), $cl)}
		</div>
	</div>
{/each}

<style lang="scss">
	@import '../color';
</style>
