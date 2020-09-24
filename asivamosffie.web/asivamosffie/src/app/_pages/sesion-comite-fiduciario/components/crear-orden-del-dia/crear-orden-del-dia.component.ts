import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { SolicitudesContractuales, SesionComiteTema, EstadosComite, ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { forkJoin } from 'rxjs';
import { ColumnasTabla, ComiteFiduciario, DataTable, SolicitudContractual } from 'src/app/_interfaces/comiteFiduciario.interfaces';
import { DatePipe } from '@angular/common';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent implements OnInit {

  solicitudesContractuales: SolicitudesContractuales[] = [];
  //Data que sera recibida del servicio
  dataSolicitudContractual: SolicitudesContractuales[] = [];
  fechaSesion: Date;
  idSesion: number;
  detalle: string = 'Crear orden del día de comité fiduciario';
  temas: string[] = [ 'Solicitudes contractuales', 'Tema nuevo' ];
  solicitudBoolean: boolean = false;
  temaNuevoBoolean: boolean = false;
  tipoDeTemas: FormControl = new FormControl();
  solicitudesSeleccionadas = [];
  estadosComite = EstadosComite;
  objetoSesion: ComiteFiduciario = {
     estadoComiteCodigo: this.estadosComite.sinConvocatoria 
   };

  addressForm = this.fb.group({
    tema: this.fb.array([]),
  });
  


  responsablesArray: Dominio[] = [];

  constructor ( private fb: FormBuilder,
                public dialog: MatDialog,
                private activatedRoute: ActivatedRoute,
                private router: Router,
                private datepipe: DatePipe,
                private fiduciaryCommitteeSession : FiduciaryCommitteeSessionService, 
                private commonService: CommonService,
                
                ) 
  {
    this.getFecha();
  };


  ngOnInit(): void {
    this.idSesion = Number( this.activatedRoute.snapshot.params.id );
    this.getSolicitudesContractuales()
  }

  //Metodo para obtener la fecha recibida del componente ordenes del dia
  getFecha () {

    if ( this.router.getCurrentNavigation().extras.replaceUrl ) {
      this.router.navigateByUrl( '/comiteFiduciario' );
      return;
    };
  
    this.fechaSesion = this.router.getCurrentNavigation().extras.state.fecha;
  
  };

  //Obteniendo valores booleanos para habilitar/deshabilitar "Solicitudes contractuales/Temas nuevos"
  getvalues ( values: string[] ) {

    const solicitud = values.find( value => value === this.temas[0] );
    const temaNuevo = values.find( value => value === this.temas[1] );
  
    solicitud ? this.solicitudBoolean = true : this.solicitudBoolean = false;
    if ( temaNuevo ) {
      this.temaNuevoBoolean = true;
    } else {
      this.temaNuevoBoolean = false
    };
  
  };

  //Obtener data de sesiones de solicitudes contractuales
  getSolicitudesContractuales () {

    forkJoin([
      this.commonService.listaMiembrosComiteTecnico(),      
      this.fiduciaryCommitteeSession.getCommitteeSessionFiduciario(),
    ]).subscribe( response => {
      this.responsablesArray = response[0];
      this.dataSolicitudContractual = response[1];  
    })

  };

  //Contador solicitudes seleccionadas
  totalSolicitudes () {

    let contador = 0;
  
    for ( let solicitud of this.solicitudesSeleccionadas ) {
  
      contador += solicitud.data.length;
  
    };
  
    return contador;
  
  }

  //Metodo para recibir las solicitudes contractuales
  getSesionesSeleccionada ( event: DataTable ) {

    if ( event.estado ) {

      const index = this.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );

      if ( index === -1 ) {
        this.solicitudesSeleccionadas.push( event.solicitud );
      } else {
        this.solicitudesSeleccionadas.splice( index, 1, event.solicitud );
      }


    } else {

      if ( event.solicitud.data.length === 0 ) {

        const index = this.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );

        this.solicitudesSeleccionadas.splice( index, 1 );

      } else {

        const index = this.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );
        this.solicitudesSeleccionadas.splice( index, 1, event.solicitud );

      }
      
    };

    console.log( 'solicitudes seleccionadas: ', this.solicitudesSeleccionadas );

  };

  editMode(){

    this.detalle = 'Ver detalle/Editar orden del día';

  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get tema() {
    return this.addressForm.get('tema') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  crearTema() {
    return this.fb.group({
      sesionTemaId: [],
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        //Validators.required,
        //Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  onSubmit() {

    console.log(this.addressForm);
    if (this.addressForm.invalid) {
      this.openDialog('Falta registrar información', '');

    }else{
      let sesion: ComiteTecnico = {
        comiteTecnicoId: this.idSesion,
        fechaOrdenDia: this.fechaSesion,
        sesionComiteTema: [],
        sesionComiteSolicitudComiteTecnico: this.solicitudesSeleccionadas
      }
  
      this.tema.controls.forEach( control => {
        let sesionComiteTema: SesionComiteTema = {
          sesionTemaId: control.get('sesionTemaId').value,
          comiteTecnicoId: this.idSesion, 
          tema: control.get('tema').value,
          responsableCodigo: control.get('responsable').value.codigo,
          tiempoIntervencion: control.get('tiempoIntervencion').value,
          rutaSoporte: control.get('url').value,
          
        }
  
        sesion.sesionComiteTema.push( sesionComiteTema );
      })

      console.log( sesion );
      
      this.fiduciaryCommitteeSession.createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud( sesion )
        .subscribe( respuesta => {
          this.openDialog( '', respuesta.message);
          if ( respuesta.code == "200" )
            this.router.navigate(['/comiteFiduciario'])          
        })

    }
  }
}
