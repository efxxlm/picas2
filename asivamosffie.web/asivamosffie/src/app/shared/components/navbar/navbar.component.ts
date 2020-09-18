import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor( private autenticacionService: AutenticacionService,
               private router: Router
             ) { }

  ngOnInit(): void {
  }

  logout() {
    this.autenticacionService.logout();
    
  }

}
