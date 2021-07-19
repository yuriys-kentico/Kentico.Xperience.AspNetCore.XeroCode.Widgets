export interface IWidgetProperty {
  id: string;
  formComponentIdentifier: string;
  name: string;
  label: string;
  defaultValue: any;
  explanationText: string;
  tooltip: string;
  order?: number;
}
