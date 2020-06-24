import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

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
