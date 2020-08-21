import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { SolicitudContractual, ComiteFiduciario, Temas, DataTable, ColumnasTabla } from '../../../../_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-crear-orden',
  templateUrl: './crear-orden.component.html',
  styleUrls: ['./crear-orden.component.scss']
})
export class CrearOrdenComponent implements OnInit {

  //Arreglo tipos de temas para crear Comite Fiduciario
  //Preguntar si sera recibida esta data en el servicio
  temas: string[] = [ 'Solicitudes contractuales', 'Tema nuevo' ];
  tipoDeTemas: FormControl = new FormControl();
  //Values Booleanos para habilitar/deshabilitar "Solicitudes contractuales/Temas nuevos"
  solicitudBoolean: boolean = false;
  temaBoolean: boolean = false;
  //Fecha recibida del componente ordenes del dia
  fechaRecibida: Date;
  //Contador de temas creados
  temaContador: number = 0;
  //Data que sera enviada a la tabla
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'seleccionar' ];
  columnas: ColumnasTabla[] = [
    { titulo: 'Fecha de la solicitud', name: 'fechaSolicitud' },
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' },
  ];
  //Data que sera recibida del servicio
  dataSolicitudContractual: SolicitudContractual[] = [];
  //Objeto con la data del comite fiduciario
  sesionComiteFiduciario: ComiteFiduciario = {
    solicitudesSeleccionadas: [],
    numeroComite: '',
    estadoComite: false,
    proposiciones: '',
    temas: [],
    fechaComite: null
  };
  //Listado de los responsables del tema a tratar
  listadoResponsables: string[] = [];

  constructor ( private routes: Router, 
                private datepipe: DatePipe ) {
    //Obteniendo fecha recibida del componente ordenes del dia
    this.obtenerFecha();
  };

  ngOnInit(): void {
    this.getListadoResponsables();
    this.getSolicitudesContractuales();
  };

  //Obtener listado responsable
  getListadoResponsables () {
    this.listadoResponsables = [
      'Dirección administrativa',
      'Dirección ténica'
    ];
  };

  //Obtener data de sesiones de solicitudes contractuales
  getSolicitudesContractuales () {
    this.dataSolicitudContractual = [
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
      },
      {
        nombreSesion: 'CT_00002',
        fecha: '30/06/2020',
        data: [
          {
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: 'PI0004',
            tipoSolicitud: 'Contratación'
          },
          {
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: '000003',
            tipoSolicitud: 'Modificación contractual'
          },
        ]
      },
      {
        nombreSesion: 'CT_00303',
        fecha: '28/06/2020',
        data: [
          {
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: 'PI0020',
            tipoSolicitud: 'Contratación - 1'
          },
          {
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: '000050',
            tipoSolicitud: 'Modificación contractual - 1'
          },{
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: 'PI0020',
            tipoSolicitud: 'Contratación - 1'
          },
          {
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: '000050',
            tipoSolicitud: 'Modificación contractual - 1'
          },{
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: 'PI0020',
            tipoSolicitud: 'Contratación - 1'
          },
          {
            fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
            numeroSolicitud: '000050',
            tipoSolicitud: 'Modificación contractual - 1'
          },
        ]
      }
    ];
  };

  //Metodo para obtener la fecha recibida del componente ordenes del dia
  obtenerFecha () {

    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl( '/comiteFiduciario' );
      return;
    };

    this.fechaRecibida = this.routes.getCurrentNavigation().extras.state.fecha;
    //Cambiar fecha a tipo Date en la interface cuando termine pruebas
    this.sesionComiteFiduciario.fechaComite = `${ this.fechaRecibida }`;

  };

  //Obteniendo valores booleanos para habilitar/deshabilitar "Solicitudes contractuales/Temas nuevos"
  values ( values: string[] ) {

    const solicitud = values.find( value => value === this.temas[0] );
    const temaNuevo = values.find( value => value === this.temas[1] );

    solicitud ? this.solicitudBoolean = true : this.solicitudBoolean = false;
    
    if ( temaNuevo ) {
      this.agregarTema();
      this.temaBoolean = true;
    } else {
      this.temaContador = 0;
      this.sesionComiteFiduciario.temas = [];
      this.temaBoolean = false;
    };

  };

  //Agregar tema a tratar en el comite
  agregarTema () {
    this.temaContador++;
    this.sesionComiteFiduciario.temas.push({
      numeroTema: this.temaContador,
      tiempoIntervencion: null,
      responsable: '',
      urlSoporte: '',
      temaTratar: ''
    });
  };
  
  //Eliminar tema a tratar en el comite
  eliminarTema ( tema: Temas ) {
    const index = this.sesionComiteFiduciario.temas.findIndex( data => data.numeroTema === tema.numeroTema );
    this.sesionComiteFiduciario.temas.splice( index, 1 );
  };

  //Metodo para recibir las solicitudes contractuales
  getSesionesSeleccionada ( event: DataTable ) {

    if ( event.estado ) {

      const index = this.sesionComiteFiduciario.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );

      if ( index === -1 ) {
        this.sesionComiteFiduciario.solicitudesSeleccionadas.push( event.solicitud );
      } else {
        this.sesionComiteFiduciario.solicitudesSeleccionadas.splice( index, 1, event.solicitud );
      }


    } else {

      if ( event.solicitud.data.length === 0 ) {

        const index = this.sesionComiteFiduciario.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );

        this.sesionComiteFiduciario.solicitudesSeleccionadas.splice( index, 1 );

      } else {

        const index = this.sesionComiteFiduciario.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );
        this.sesionComiteFiduciario.solicitudesSeleccionadas.splice( index, 1, event.solicitud );

      }
      
    };

    console.log( 'solicitudes seleccionadas: ', this.sesionComiteFiduciario.solicitudesSeleccionadas );

  };

  //Contador solicitudes seleccionadas
  totalSolicitudes () {

    let contador = 0;

    for ( let solicitud of this.sesionComiteFiduciario.solicitudesSeleccionadas ) {

      contador += solicitud.data.length;

    };

    return contador;

  }

  //Enviar data del comite fiduciario

  enviarSesion () {
    console.log( this.sesionComiteFiduciario );
  };

};