import App from './app.svelte';

const app = new App({
  target: document.getElementById('widgetsRoot'),
  props: {
    resources: JSON.parse((document.getElementById('widgetsResources') as HTMLInputElement).value),
    endpoints: JSON.parse((document.getElementById('widgetsEndpoints') as HTMLInputElement).value),
  },
});

export default app;
