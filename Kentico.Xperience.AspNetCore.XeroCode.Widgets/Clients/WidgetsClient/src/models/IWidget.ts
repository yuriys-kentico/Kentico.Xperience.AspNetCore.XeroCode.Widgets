import type { IWidgetProperty } from './IWidgetProperty';

export interface IWidget {
  id: number;
  guid: string;
  name: string;
  description: string;
  icon: string;
  view: string;
  properties: IWidgetProperty[];
}
