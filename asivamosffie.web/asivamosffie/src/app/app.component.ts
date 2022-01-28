import { Component } from '@angular/core';
import { CommonService } from './core/_services/common/common.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  title = 'asivamosffie';

  constructor(
    private commonSvc: CommonService
    ){}

  ngOnInit(): void {
    this.commonSvc.getVersion().subscribe( result => { console.log("Versión Front: "+ result.front + " Back: " + result.back);});
  }
}
