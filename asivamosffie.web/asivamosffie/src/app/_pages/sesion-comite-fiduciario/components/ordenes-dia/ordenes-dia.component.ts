import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ColumnasTabla, ComiteFiduciario } from 'src/app/_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-ordenes-dia',
  templateUrl: './ordenes-dia.component.html',
  styleUrls: ['./ordenes-dia.component.scss']
})
export class OrdenesDiaComponent implements OnInit {

  fechaComiteFiduciario: Date;
  minDate: Date;

  displayedColumns: string[] = [ 'fechaComite', 'numeroComite', 'estadoComite', 'gestion' ];
  columnas: ColumnasTabla[] = [
    { titulo: 'Fecha de comité', name: 'fechaComite' },
    { titulo: 'Número de comité', name: 'numeroComite' }
  ];

  sesionComiteFiduciario: ComiteFiduciario[] = [
    
  ];

  constructor ( private routes: Router,
                private datepipe: DatePipe ) {
    this.minDate = new Date();
  };

  ngOnInit(): void {
    this.getComiteFiduciario();
  };

  getCrearOrden () {

    if ( !this.fechaComiteFiduciario ) return;

    this.routes.navigate( ['/comiteFiduciario/crearOrden'], { state: { fecha: this.fechaComiteFiduciario } } );

  };

  getComiteFiduciario () {

    this.sesionComiteFiduciario.push( 
      {
        solicitudesSeleccionadas: [
          {
            nombreSesion: 'CT_00001',
            fecha: '24/06/2020',
            data: [
              {
                fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
                numeroSolicitud: 'SC0005',
                tipoSolicitud: 'Evaluación de proceso de selección'
              }
            ]
          }
        ],
        numeroComite: 'CF_00001',
        estadoComite: false,
        proposiciones: '',
        temas: [
          {
            numeroTema: 1,
            tiempoIntervencion: 45,
            responsable: 'Dirección técnica',
            urlSoporte: 'DestoktopFFIE/archivosgenerales/jmgarzon/comite/temas/revisiondeterminostecnicos',
            temaTratar: 'Revisión de terminos técnicos'
          }
        ],
        fechaComite: this.datepipe.transform( new Date(), 'dd-MM-yyyy')
      },
      {
        solicitudesSeleccionadas: [],
        numeroComite: 'CF_00002',
        estadoComite: false,
        proposiciones: '',
        temas: [],
        fechaComite: this.datepipe.transform( new Date(), 'dd-MM-yyyy')
      }
    );

  };

};
