import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { UrlResolver } from '@angular/compiler';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
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
      title: 'Gestionar acuerdo de cofinanciación',
      link: '/gestionarAcuerdos'
    },
    {
      title: 'Gestionar fuentes de financiación',
      link: '/gestionarFuentes'
    },
    {
      title: 'Crear proyecto técnico',
      link: '/crearProyecto'
    },
    {
      title: 'Crear proyecto administrativo',
      link: '/crearProyectoAdministrativo'
     
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
  menu: any[]=[];

  constructor(private authe: AutenticacionService,private common: CommonService,private router: Router) {
    this.actualUser = this.authe.actualUser;
  }

  actualUser: any;
  ngOnInit(): void {
    this.authe.actualUser$.subscribe(user => { 
      console.log(user);
      if(user==null)
      {
        console.log("iniciando");        
      }
      else{
        this.actualUser = user;         
        this.getMenu();
        if(user.fechaUltimoIngreso==null || user.cambiarContrasena)
        {        
          this.router.navigate(['/cambiarContrasena']);
        }
      }      
    });
  }
  getMenu()
  {
    this.common.loadMenu().subscribe(data => { 
      data.forEach(element => {
        //console.log(element);
        this.menu.push({title:element.menu.nombre,link:element.menu.rutaFormulario});
      });
      this.menu.push({title:'Gestionar procesos contractuales',link:'/procesosContractuales'})
      console.log(this.menu);
    });
  }

}
