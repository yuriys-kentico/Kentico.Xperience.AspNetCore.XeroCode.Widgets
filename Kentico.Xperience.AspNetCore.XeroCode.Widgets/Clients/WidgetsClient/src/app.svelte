<script lang="ts" context="module">
  export const contextKey = {};
</script>

<script lang="ts">
  import { setContext } from 'svelte';
  import Widgets from './components/widgets.svelte';

  export let resources: { [key: string]: string };
  export let endpoints: { allRequest: string };

  setContext(contextKey, {
    resources,
    endpoints,
  });
</script>

<main>
  <Widgets />
</main>

<style>
  main {
    overflow: auto;
    font-family: 'Segoe UI', Helvetica, Verdana, Arial, sans-serif;
    font-size: 14px;
    position: absolute;
    width: 100%;
    height: 100%;
    left: 0;
    top: 0;
    padding: 1em;
    box-sizing: border-box;
  }
</style>
