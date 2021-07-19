<script lang="ts">
  import { getContext } from 'svelte';
  import { upperFirst, camelCase } from 'lodash';

  import type { IWidgetProperty } from '../models/IWidgetProperty';

  import { contextKey } from '../app.svelte';

  import Delete from './icons/delete.svelte';
  import { formComponents } from '../formComponents';

  export let property: IWidgetProperty;
  export let deleteProperty: (propertyToDelete: IWidgetProperty) => void;
  export let validateProperty: () => void;

  $: if (property.label !== undefined) {
    property.name = upperFirst(camelCase(property.label || ''));

    if (property.name.length > 0) {
      property.name =
        property.name[0].replaceAll(/[^a-zA-Z_]/gi, '_') + property.name.slice(1).replaceAll(/[^a-zA-Z0-9_]/gi, '_');
    }

    validateProperty();
  }

  $: components = formComponents(resources);
  $: component = components[property.formComponentIdentifier];

  const { resources } = getContext(contextKey);
</script>

<div class="root group">
  <div class="controls group">
    <div class="button delete" on:click={() => deleteProperty(property)}><Delete /></div>
  </div>
  <div class="component group column">
    <label class="group column">
      <span><b>{resources['xerocode.widgets.property.componentlabel']}</b></span>
      <!-- svelte-ignore a11y-no-onchange -->
      <select
        bind:value={property.formComponentIdentifier}
        on:change={() => {
          property.defaultValue = components[property.formComponentIdentifier].defaultValue;
        }}
      >
        {#each Object.entries(components) as [name, formComponent]}
          <option value={name}>{formComponent.label}</option>
        {/each}
      </select>
    </label>
    <!-- svelte-ignore a11y-label-has-associated-control -->
    <label class="group column">
      <span><b>{resources['xerocode.widgets.property.defaultvaluelabel']}</b></span>
      {#if component.type === 'number'}
        <input bind:value={property.defaultValue} type="number" step="1" />
      {:else if component.type === 'checkbox'}
        <input bind:checked={property.defaultValue} type="checkbox" />
      {:else if component.type === 'tel'}
        <input bind:value={property.defaultValue} type="tel" />
      {:else if component.type === 'email'}
        <input bind:value={property.defaultValue} type="email" />
      {:else}
        <input bind:value={property.defaultValue} type="text" />
      {/if}
    </label>
  </div>
  <div class="label group column item">
    <label class="group column">
      <span><b>{resources['xerocode.widgets.property.labellabel']}</b></span>
      <input bind:value={property.label} />
    </label>
    <label class="group column">
      <span><b>{resources['xerocode.widgets.property.namelabel']}</b></span>
      <input bind:value={property.name} disabled />
    </label>
  </div>
  <div class="tooltip group column item">
    <label class="group column">
      <span><b>{resources['xerocode.widgets.property.tooltiplabel']}</b></span>
      <input bind:value={property.tooltip} />
    </label>
    <label class="group column">
      <span><b>{resources['xerocode.widgets.property.explanationtextlabel']}</b></span>
      <input bind:value={property.explanationText} />
    </label>
  </div>
</div>

<style>
  .group {
    display: flex;
  }

  .column {
    flex-direction: column;
  }

  .item {
    flex: 1;
  }

  .root {
    border-bottom: 1px solid #e5e5e5;
    border-top: 1px solid transparent;
    align-items: center;
  }

  .controls {
    margin: 0 0.5em 0 0;
  }

  .button {
    height: 1.5em;
    width: 1.5em;
    padding: 0.5em;
  }

  .button.delete {
    color: #b12628;
  }

  .button:hover {
    color: #283f8a;
    cursor: pointer;
  }

  input,
  select {
    width: 100%;
    height: 2em;
    margin: 0;
    padding: 4px 12px;
    font-size: 14px;
    color: #262524;
    vertical-align: baseline;
    background-color: #f3f4f5;
    border: 2px solid #a1a9b6;
    border-radius: 16px;
    text-overflow: ellipsis;
    box-sizing: border-box;
    transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
  }

  input:focus,
  select:focus {
    border-color: #7d95e3;
    background-color: #fff;
    outline: 0;
  }

  input:disabled {
    border: none;
    background: none;
    padding: 4px 0;
  }

  label {
    padding: 4px 12px;
  }

  .component {
    margin: 0 0.5em 0 0;
    flex: 0.75;
  }

  .label {
    margin: 0 0.5em 0 0;
  }

  .tooltip {
    margin: 0 0.5em 0 0;
  }
</style>
