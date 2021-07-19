<script lang="ts">
  import { getContext, onMount } from 'svelte';
  import wretch from 'wretch';

  import '../styles/icons.css';

  import type { IWidget } from '../models/IWidget';
  import type { IWidgetProperty } from '../models/IWidgetProperty';

  import { contextKey } from '../app.svelte';

  import WidgetLine from './widgetLine.svelte';
  import { tryRequest } from '../tryRequest';

  interface IAllResponse {
    widgets: IWidget[];
  }

  interface INewPropertyResponse {
    property: IWidgetProperty;
  }

  interface INewWidgetResponse {
    widget: IWidget;
  }

  interface IValidationError {
    errors: { [key: string]: string[] };
    title: string;
  }

  let widgets: IWidget[];
  let errorMessage: string;

  const { resources, endpoints } = getContext(contextKey);

  const parseError = (error: string | Error) => {
    if (error instanceof Error) {
      const parsedError = JSON.parse(error.message) as IValidationError;

      errorMessage = `${parsedError.title} ${Object.values(parsedError.errors).flat().join(' ')}`;
    } else {
      errorMessage = new String(error).split('\n')[0];
    }
  };

  const allRequest = tryRequest(() => {
    errorMessage = undefined;
    return wretch(endpoints.allRequest).post().json<IAllResponse>();
  }, parseError);

  const upsertRequest = tryRequest((widget: IWidget) => {
    errorMessage = undefined;
    return wretch(endpoints.upsertRequest).post(widget).text();
  }, parseError);

  const removeRequest = tryRequest((widget: IWidget) => {
    errorMessage = undefined;
    return wretch(endpoints.removeRequest).post(widget).text();
  }, parseError);

  const newPropertyRequest = tryRequest(() => {
    errorMessage = undefined;
    return wretch(endpoints.newPropertyRequest).post().json<INewPropertyResponse>();
  }, parseError);

  const newWidgetRequest = tryRequest(() => {
    errorMessage = undefined;
    return wretch(endpoints.newWidgetRequest).post().json<INewWidgetResponse>();
  }, parseError);

  onMount(async () => {
    const response = await allRequest();

    widgets = response.widgets;
  });
</script>

<div class="root group item column">
  <h1>
    {resources['xerocode.widgets.widgets']}
  </h1>
  <div class="container group">
    {#if widgets !== undefined}
      <div class="list column item">
        {#each widgets as widget (widget.guid)}
          <WidgetLine
            {widget}
            update={async (widget) => {
              for (let propertyIndex = 0; propertyIndex < widget.properties.length; propertyIndex++) {
                widget.properties[propertyIndex].order = propertyIndex;
              }

              await upsertRequest(widget);

              if (errorMessage !== undefined) {
                return false;
              }

              return true;
            }}
            removeWidget={async (widget) => {
              await removeRequest(widget);

              widgets = widgets.filter((oldWidget) => oldWidget.guid !== widget.guid);
            }}
            newProperty={async () => {
              const response = await newPropertyRequest();

              return response.property;
            }}
          />
        {/each}
      </div>
    {/if}
  </div>
  <div class="group">
    <button
      on:click={async () => {
        const response = await newWidgetRequest();

        widgets = [...widgets, response.widget];
      }}>{resources['xerocode.widgets.addnew']}</button
    >
    {#if errorMessage !== undefined}
      <div class="validation item">
        <span>
          {errorMessage}
        </span>
      </div>
    {/if}
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
    height: 100%;
  }

  h1 {
    margin: 0.2em 0 0.5em 0;
  }

  .container {
    overflow-y: auto;
  }

  button {
    color: #fff;
    background-color: #3655ba;
    display: inline-block;
    padding: 0 24px;
    margin: 0.5em 0 0;
    font-size: 14px;
    font-family: 'Segoe UI Semibold', Helvetica, Verdana, Arial, sans-serif;
    font-weight: 600;
    line-height: 32px;
    width: auto;
    height: 32px;
    text-align: center;
    vertical-align: middle;
    text-decoration: none;
    cursor: pointer;
    border: none;
    border-radius: 16px;
    white-space: nowrap;
  }

  button:hover {
    background-color: #283f8a;
  }

  button:focus {
    outline-color: transparent;
    outline: none;
  }

  .validation {
    padding: 0 0.5em;
    margin: 0.5em 0 0;
    align-self: center;
    color: #b12628;
  }

  .validation:before {
    font-family: 'Core-icons';
    font-size: 16px;
    content: '';
    color: #e14344;
    margin-right: 6px;
  }
</style>
