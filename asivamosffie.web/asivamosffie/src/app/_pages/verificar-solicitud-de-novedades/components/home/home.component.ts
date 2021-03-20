import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  verAyuda = false;

  obra = false;
  interventoria = false;

  constructor( 
    private router: Router,
     
  ) { }

  ngOnInit(): void {
  }

  registrarSolicitud(){
    this.router.navigate(["/verificarSolicitudDeNovedades/registrarSolicitudInterventoria",0])
  }


}
