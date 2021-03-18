import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';

@Component({
  selector: 'app-layout-reportes',
  templateUrl: './layout-reportes.component.html',
  styleUrls: ['./layout-reportes.component.scss']
})
export class LayoutReportesComponent implements OnInit {

  constructor(private authe: AutenticacionService) {
    this.actualUser = this.authe.actualUser;
  }

  actualUser: any;

  ngOnInit(): void {
    this.authe.actualUser$.subscribe(user => {
      this.actualUser = user;
    });
  }

}
