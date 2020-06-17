import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommonService } from './_services/common/common.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    CommonService
  ]
})
export class CoreModule { }
