import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import {ComponentCanDeactivate} from './component-can-deactivate';

@Injectable()
export class CanDeactivateGuard implements CanDeactivate<ComponentCanDeactivate> {
  canDeactivate(component: ComponentCanDeactivate): boolean {
   //por ahora lo comento jflorez
    /*if(!component.canDeactivate()){
        if (confirm("Si abandona esta pantalla se perderán los cambios.")) {
            return true;
        } else {
            //component.saveMe();
            return false;
        }
    }*/
    return true;
  }
}
