import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

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
