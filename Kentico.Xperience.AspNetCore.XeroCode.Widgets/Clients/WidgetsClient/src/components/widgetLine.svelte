<script lang="ts">
  import clsx from 'clsx';
  import { getContext } from 'svelte';
  import { fade } from 'svelte/transition';

  import type { IWidget } from '../models/IWidget';

  import { contextKey } from '../app.svelte';

  import WidgetProperty from './widgetProperty.svelte';
  import IconsList from './IconsList.svelte';
  import Edit from './icons/edit.svelte';
  import Delete from './icons/delete.svelte';
  import Check from './icons/check.svelte';
  import Cancel from './icons/cancel.svelte';
  import Editor from './editor.svelte';
  import type { IWidgetProperty } from '../models/IWidgetProperty';

  export let widget: IWidget;
  export let update: (widget: IWidget) => Promise<boolean>;
  export let removeWidget: (widget: IWidget) => void;
  export let newProperty: () => Promise<IWidgetProperty>;

  let validPropertiesMessage: string;
  let validNameMessage: string;
  let editing: boolean = false;
  let oldWidget: IWidget | undefined;
  let propertiesList: HTMLDivElement;

  $: validProperties = validPropertiesMessage === undefined;
  $: validName = validNameMessage === undefined;
  $: valid = validProperties && validName;

  const validateProperties = () => {
    for (const property of widget.properties) {
      if (property.name === '') {
        validPropertiesMessage = resources['xerocode.widgets.properties.invalid'];
        return;
      }

      for (const otherProperty of widget.properties) {
        if (property !== otherProperty && property.name === otherProperty.name) {
          validPropertiesMessage = resources['xerocode.widgets.properties.invalid'];
          return;
        }
      }
    }

    validPropertiesMessage = undefined;
  };

  const validateName: svelte.JSX.FormEventHandler<HTMLInputElement> = (event) => {
    if (event?.currentTarget.value === '') {
      validNameMessage = resources['xerocode.widgets.name.invalid'];
      return;
    }

    validNameMessage = undefined;
  };

  const { resources } = getContext(contextKey);
</script>

<div class="root group" class:editing>
  <div class="controls group">
    {#if editing}
      <div
        class="button edit"
        class:disabled={!valid}
        title={resources['xerocode.widgets.savechanges']}
        on:click={async () => {
          if (valid) {
            if (await update(widget)) {
              editing = false;
              oldWidget = undefined;
            }
          }
        }}
      >
        <Check />
      </div>
      <div
        class="button delete"
        title={resources['xerocode.widgets.discardchanges']}
        on:click={() => {
          editing = false;
          widget = oldWidget;
          validateProperties();
          validateName(undefined);
          oldWidget = undefined;
        }}
      >
        <Cancel />
      </div>
    {:else}
      <div
        class="button edit"
        title={resources['xerocode.widgets.edit']}
        on:click={() => {
          editing = true;
          oldWidget = { ...widget, properties: widget.properties.map((property) => ({ ...property })) };
        }}
      >
        <Edit />
      </div>
      <div
        class="button delete"
        title={resources['xerocode.widgets.delete']}
        on:click={() => {
          removeWidget(widget);
        }}
      >
        <Delete />
      </div>
    {/if}
  </div>
  {#if editing}
    <div class="icons group column">
      <span><b>{resources['xerocode.widgets.iconlabel']}</b></span>
      <IconsList bind:selectedIcon={widget.icon} />
    </div>
  {:else}
    <div class={clsx('icon', widget.icon)} title={widget.icon} />
  {/if}
  <div class="fields group column item">
    <div class="top group">
      <div class="name">
        {#if editing}
          <label>
            <span><b>{resources['xerocode.widgets.namelabel']}</b></span>
            <input bind:value={widget.name} on:input={validateName} />
          </label>
        {:else}
          <span>
            {widget.name}
          </span>
        {/if}
      </div>
      {#if !validName}
        <div class="validation lower">
          <span>
            {validNameMessage}
          </span>
        </div>
      {/if}
      <div class="description item">
        {#if editing}
          <label>
            <span><b>{resources['xerocode.widgets.descriptionlabel']}</b></span>
            <input bind:value={widget.description} />
          </label>
        {:else}
          <span>
            {widget.description}
          </span>
        {/if}
      </div>
    </div>

    {#if editing}
      <div class="bottom group">
        <div class="properties group column item">
          <h3>{resources['xerocode.widgets.properties']}</h3>
          <div bind:this={propertiesList} class="list">
            {#each widget.properties as property (property.id)}
              <div transition:fade>
                <WidgetProperty
                  {property}
                  deleteProperty={(property) => {
                    widget.properties = widget.properties.filter((oldProperty) => oldProperty.id !== property.id);
                    validateProperties();
                  }}
                  validateProperty={validateProperties}
                />
              </div>
            {/each}
          </div>
          <div class="group">
            <button
              on:click={async () => {
                widget.properties = [...widget.properties, await newProperty()];

                setTimeout(() => {
                  propertiesList.lastElementChild.scrollIntoView();
                }, 100);
              }}>{resources['xerocode.widgets.properties.addnew']}</button
            >
            {#if !validProperties}
              <div class="validation item">
                <span>
                  {validPropertiesMessage}
                </span>
              </div>
            {/if}
          </div>
        </div>
        <div class="view item">
          <h3>{resources['xerocode.widgets.view']}</h3>
          <div class="item">
            <Editor value={widget.view} updateValue={(value) => (widget.view = value)} />
          </div>
        </div>
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
    border-bottom: 1px solid #e5e5e5;
    border-top: 1px solid transparent;
    height: 2.4em;
    overflow: hidden;
  }

  .root.editing {
    height: 28.5em;
  }

  .controls {
    margin: 0 0.5em 0 0;
  }

  .button {
    height: 1.5em;
    width: 1.5em;
    padding: 0.5em;
  }

  .button.edit {
    color: #497d04;
  }

  .button.edit.disabled {
    color: grey;
    cursor: default;
  }

  .button.delete {
    color: #b12628;
  }

  .button:hover {
    color: #283f8a;
    cursor: pointer;
  }

  .icon {
    height: 1.8em;
    line-height: 1.4em;
    padding: 0.1em;
    width: 1.8em;
    text-align: center;
    box-sizing: border-box;
    border: 1px solid #e5e5e5;
    border-radius: 0.2em;
    margin: 0.1em;
    align-self: center;
  }

  .icons {
    border: 1px solid #e5e5e5;
    border-radius: 0.2em;
    padding: 0 0.2em;
    margin: 0.5em 0;
  }

  .name {
    margin: 0.5em;
    font-weight: 500;
  }

  .description {
    margin: 0.5em;
  }

  input {
    width: 100%;
    margin: 0.2em 0 0;
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

  input:focus {
    border-color: #7d95e3;
    background-color: #fff;
    outline: 0;
  }

  .top {
    height: 4em;
  }

  .bottom {
    height: calc(100% - 4em);
  }

  h3 {
    margin: 0 0 0.5em 0;
  }

  .properties {
    border: 1px solid #e5e5e5;
    border-radius: 0.2em;
    margin: 0.5em;
    padding: 0.5em;
  }

  .properties .list {
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
    color: #b12628;
    align-self: center;
  }

  .validation.lower {
    margin-top: 2em;
  }

  .validation:before {
    font-family: 'Core-icons';
    font-size: 16px;
    content: '';
    color: #e14344;
    margin-right: 6px;
  }

  .view {
    border: 1px solid #e5e5e5;
    border-radius: 0.2em;
    margin: 0.5em;
    padding: 0.5em;
  }
</style>
