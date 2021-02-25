import { Component, OnInit } from '@angular/core';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-menu-generar-disponibilidad',
  templateUrl: './menu-generar-disponibilidad.component.html',
  styleUrls: ['./menu-generar-disponibilidad.component.scss']
})
export class MenuGenerarDisponibilidadComponent implements OnInit {

  verAyuda = false;
  listaDisponibilidades: any[] = [];

  constructor(private disponibilidadServices: DisponibilidadPresupuestalService) { }

  ngOnInit(): void {
    this.disponibilidadServices.GetListGenerarDisponibilidadPresupuestal().subscribe(respuesta => {
      this.listaDisponibilidades = respuesta;

      this.listaDisponibilidades.forEach(element => {
        //determino si esta completo o incompleto

        if (element.nombreEstado == 'Con validación presupuestal') {
          element.completo = 'Completo';

          element.disponibilidadPresupuestal.forEach(element2 => {

            // esta validacion le cambia el estado mas adelante
            if (element2.numeroDdp == null) {
              if (element2.estadoRegistro) {
                element2.estadoRegistro = false;
              }
            }

            if (element2.estadoRegistro !== true) {
              element.completo = 'Incompleto';
            }
          });
        }

      });
    }
    );
  }

}
