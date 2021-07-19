export const formComponents = (resources: { [key: string]: string }) => ({
  'Kentico.TextInput': {
    label: resources['xerocode.widgets.formcomponents.textinput'],
    type: 'text',
    defaultValue: '',
  },
  'Kentico.TextArea': {
    label: resources['xerocode.widgets.formcomponents.textarea'],
    type: 'text',
    defaultValue: '',
  },
  'Kentico.EmailInput': {
    label: resources['xerocode.widgets.formcomponents.emailinput'],
    type: 'email',
    defaultValue: '',
  },
  'Kentico.IntInput': {
    label: resources['xerocode.widgets.formcomponents.intinput'],
    type: 'number',
    defaultValue: null,
  },
  'Kentico.CheckBox': {
    label: resources['xerocode.widgets.formcomponents.checkbox'],
    type: 'checkbox',
    defaultValue: false,
  },
  'Kentico.USPhone': {
    label: resources['xerocode.widgets.formcomponents.usphone'],
    type: 'tel',
    defaultValue: '',
  },
});
