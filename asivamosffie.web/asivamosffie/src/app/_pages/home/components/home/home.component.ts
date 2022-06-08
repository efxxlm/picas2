import { Component, OnInit } from '@angular/core';
import { AutenticacionService } from 'src/app/core/_services/autenticacion/autenticacion.service';
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
  optionsMenu: any[];
  menuFaseInicio: any[] = [];
  menuFaseSeguimiento: any[] = [];
  menuFaseCierre: any[] = [];

  mostrarMenuFaseInicio = false;
  mostrarMmenuFseSeguimiento = false;
  mostrarMenuFaseCierre = false;

  constructor(private authe: AutenticacionService, private common: CommonService, private router: Router) {
    // this.actualUser = this.authe.actualUser;
  }

  // actualUser: any;
  ngOnInit(): void {
    this.authe.actualUser$.subscribe(user => {
      if (user == null) {
        // console.log("iniciando");
      } else {
        // this.actualUser = user;
        this.getMenu();
        if (user.fechaUltimoIngreso == null || user.cambiarContrasena) {
          this.router.navigate(['/cambiarContrasena']);
        }
      }
    });
  }

  getMenu() {
    this.common.loadMenu().subscribe(data => {
      console.log(data);
      this.optionsMenu = data;
      data.forEach(element => {
        if (element.menu.faseCodigo === '1') {
          this.menuFaseInicio.push({ title: element.menu.nombre, link: element.menu.rutaFormulario });
        } else if (element.menu.faseCodigo === '2') {
          this.menuFaseSeguimiento.push({ title: element.menu.nombre, link: element.menu.rutaFormulario });
        } else if (element.menu.faseCodigo === '3') {
          this.menuFaseCierre.push({ title: element.menu.nombre, link: element.menu.rutaFormulario });
        }
      });
      //this.menu.push({title:'Gestionar procesos contractuales',link:'/procesosContractuales'})
      //this.menu.push({title:'Gestionar compromisos y actas de comités',link:'/compromisosActasComite'});
    });
  }
}
