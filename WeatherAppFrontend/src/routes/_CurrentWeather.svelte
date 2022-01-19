<script lang="ts">
	import l from '../localizer';
	import { currentLanguage as cl } from '../stores';
	import weatherDescriptions from '../weatherDescriptions';
	import degreeRotationToDirections from '../degreeRotationToDirections';
	import timestampToString from '../timestampToString';

	export let firstWeatherItem = undefined;
	export let weatherIconPath = '';

	$: weatherName = (() => {
		return typeof firstWeatherItem === 'undefined' ? 'clearsky_day' : firstWeatherItem.weatherType;
	})();

	function getWeatherDescription(weatherName: string, currentLanguage: string) {
		weatherName = weatherName.split('_')[0];

		let weatherDescription = weatherDescriptions[weatherName];
		if (!weatherDescription) {
			return l('Unknown Weather', currentLanguage);
		}
		return weatherDescription['desc_'.concat(currentLanguage)];
	}

	$: weatherDescription = (() => {
		let weatherType = firstWeatherItem?.weatherType;
		if (typeof weatherType === 'undefined') {
			weatherType = 'clearsky_day';
		}
		return getWeatherDescription(weatherType, $cl);
	})();

	$: temperature = typeof firstWeatherItem === 'undefined' ? '???' : firstWeatherItem.temperatureC;

	$: precipitationNext6Hours = firstWeatherItem?.precipitationNext6Hours;

	$: windVelocity = firstWeatherItem?.windVelocity;

	$: windFromDirection = degreeRotationToDirections(firstWeatherItem);

	$: location =
		typeof firstWeatherItem?.location === 'undefined'
			? 'Unknown Location'
			: firstWeatherItem.location;

	$: lastUpdate = (() => {
		let lastUpdateString = firstWeatherItem?.date;
		if (typeof lastUpdateString !== 'string') return '';
		return lastUpdateString;
	})();
</script>

<div class="row rounded mt-4 pt-3 pb-3 justify-content-center weather-card-background">
	<div class="col-auto">
		<img
			alt={weatherDescription}
			src={weatherIconPath.concat(weatherName, '.png')}
			class="img-fluid"
			width="200"
			height="200"
		/>
	</div>
	<div class="col-lg">
		<div class="container-fluid">
			<div class="row">
				<div class="col">
					<h2 class="pl-4 text-center">
						{l('Weather in ', $cl)}{location}
					</h2>
				</div>
			</div>
			<div class="row">
				<div class="col">
					<h5 class="text-center">
						{l('Last updated', $cl)}: {timestampToString(new Date(lastUpdate))}
					</h5>
				</div>
			</div>
			<div class="row">
				<div class="col">
					<h1 class="pl-4 text-center">{temperature} °C</h1>
				</div>
			</div>
			<div class="row">
				<!--<div class="col">
					<h2 class="pl-4 text-center">{weatherDescription}</h2>
				</div>-->
				<div class="col">
					<div class="container-fluid">
						<div class="row justify-content-center">
							<div class="col-sm">
								<h4 class="text-center">
									{weatherDescription}
								</h4>
							</div>
							<div class="col-sm">
								<h4 class="text-center">
									{#if precipitationNext6Hours === 0}
										{l('Precipitation: None', $cl)}
									{:else if typeof precipitationNext6Hours === 'number'}
										{l('Precipitation', $cl)}: {(precipitationNext6Hours * 4).toFixed(2)}mm/{l(
											'day',
											$cl
										)}
									{/if}
								</h4>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col">
					<div class="container-fluid">
						<div class="row justify-content-center">
							<div class="col-sm">
								<h4 class="text-center">
									{l('Wind Velocity', $cl)}: {windVelocity}m/s
								</h4>
							</div>
							<div class="col-sm">
								<h4 class="text-center">
									{l('Wind From', $cl)}: {l(windFromDirection, $cl)}
								</h4>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>

<style lang="scss">
	@import '../color';
</style>
