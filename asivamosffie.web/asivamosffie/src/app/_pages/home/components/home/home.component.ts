import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';
// import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  data: any[];

  optionsMenu = [
    {
      title: 'Crear proyecto',
      link: '/#'
    },
    {
      title: 'Registrar proyectos postulados',
      link: '/#'
    },
    {
      title: 'Cargar masivamente proyectos viabilizados',
      link: '/cargarMasivamente'
    },
  ];

  constructor(private authe: AutenticacionService) {
    this.actualUser = this.authe.actualUser;
  }

  actualUser: any;
  ngOnInit(): void {
    this.authe.actualUser$.subscribe(user => {
      this.actualUser = user;
      console.log(user);
    });
  }
  probarconsumo()
  {
    this.authe.consumoePrueba().subscribe(data => { this.data = data; });
  }

}
