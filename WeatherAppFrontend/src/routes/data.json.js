import fetch from 'node-fetch';

/** @type {import('@sveltejs/kit').RequestHandler} */
export async function get(request) {
    let response = await fetch('http://localhost:5107/WeatherForecast');

    return {
        body: await response.json(),
        status: response.status
    }
}