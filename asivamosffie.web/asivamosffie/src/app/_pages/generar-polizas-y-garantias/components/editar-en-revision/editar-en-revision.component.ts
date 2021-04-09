import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';
@Component({
  selector: 'app-editar-en-revision',
  templateUrl: './editar-en-revision.component.html',
  styleUrls: ['./editar-en-revision.component.scss']
})
export class EditarEnRevisionComponent implements OnInit, OnDestroy {

    addressForm = this.fb.group({
        nombre: [ null, Validators.compose([ Validators.required, Validators.minLength(1), Validators.maxLength(50)]) ],
        numeroPoliza: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(20) ]) ],
        numeroCertificado: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(20)]) ],
        fecha: [ null, Validators.required ],
        seguros: this.fb.array( [] ),
        polizasYSeguros: [ null, Validators.required ],
        cumpleAsegurado: [ null, Validators.required ],
        cumpleBeneficiario: [ null, Validators.required],
        cumpleAfianzado: [ null, Validators.required],
        reciboDePago: [ null, Validators.required],
        condicionesGenerales: [ null, Validators.required],
        fechaRevision: [ null, Validators.required],
        estadoRevision: [ null, Validators.required],
        fechaAprob: [ null, Validators.required],
        responsableAprob: [ '', Validators.required ],
        observacionesGenerales: [ null ]
    });
    polizasYSegurosArray: Dominio[] = [];
    estadosPoliza = EstadoPolizaCodigo;
    estadoArray = [];/*
      { name: 'Devuelta', value: '1' },
      { name: 'Aprobada', value: '2' }
    ];*/
    aprobadosArray = [
      { name: 'Andres Montealegre', value: '1' },
      { name: 'David Benitez', value: '2' }
    ];
    minDate: Date;
    editorStyle = {
      height: '100px'
    };

    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };
    contrato: any;
    // Variables que se van a eliminar
    public tipoContrato;
    public objeto;
    public nombreContratista;
    public tipoIdentificacion;
    public numeroIdentificacion;
    public valorContrato;
    public plazoContrato;
    public numContrato;
    public observacionesOn;
    public idContrato;
    public idPoliza;
    public idPoliza2;
    public idObservacion;
    public selected = [];
    public obj1;
    public obj2;
    public arrayprueba = ["1", "2"];
    arrayGarantias: any[] = [];
    obj3: boolean;
    obj4: boolean;
    fechaFirmaContrato: any;
    tipoSolicitud: any;
    ultimoEstadoRevision: any;
    ultimaFechaRevision: any;
    listaUsuarios: any[] = [];
    realizoPeticion: boolean = false;
    estaEditando = false;

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private router: Router,
        private polizaService: PolizaGarantiaService,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private common: CommonService,
        private contratacion: ProjectContractingService )
    {
      this.minDate = new Date();
    }

    ngOnInit(): void {
      this.activatedRoute.params.subscribe(param => {
        this.loadContrato(param.id);
        this.loadData(param.id);
      });
    }

    ngOnDestroy(): void {
      if (this.addressForm.dirty === true && this.realizoPeticion === false) {
        this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
      }
    }

    openDialogConfirmar(modalTitle: string, modalText: string) {
      const confirmarDialog = this.dialog.open(ModalDialogComponent, {
        width: '30em',
        data: { modalTitle, modalText, siNoBoton: true }
      });

      confirmarDialog.afterClosed()
        .subscribe(response => {
          if (response === true) {
            this.onSubmit();
          }
        });
    }

    loadContrato(id) {
      this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
        this.fechaFirmaContrato = data[0].fechaFirmaContrato;
        this.tipoSolicitud = data[0].tipoSolicitud;
        this.numContrato = data[0].numeroContrato;
        this.tipoIdentificacion = data[0].tipoDocumento;
        if (data[0].objetoContrato != undefined || data[0].objetoContrato != null) {
          this.objeto = data[0].objetoContrato;
        }
        else {
          this.objeto = '';
        };
        this.tipoContrato = data[0].tipoContrato;
        if (data[0].valorContrato != undefined || data[0].valorContrato != null) {
          this.valorContrato = data[0].valorContrato;
        }
        else {
          this.valorContrato = 0;
        }
        this.plazoContrato = data[0].plazoContrato;
        this.loadContratacionId(data[0].contratacionId);
      });
      this.common.listaGarantiasPolizas().subscribe(data0 => {
        this.polizasYSegurosArray = data0;
      });
      this.common.getUsuariosByPerfil(10).subscribe(resp => {
        this.listaUsuarios = resp;
      });
      this.common.listaEstadoRevision().subscribe(resp=>{
        this.estadoArray=resp;
      })
    }

    loadData(id) {
      this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(data => {
        this.addressForm.get('nombre').setValue(data.nombreAseguradora);
        this.addressForm.get('numeroPoliza').setValue(data.numeroPoliza);
        this.addressForm.get('numeroCertificado').setValue(data.numeroCertificado);
        this.addressForm.get('observacionesGenerales').setValue( data.observacionesRevisionGeneral !== undefined ? ( data.observacionesRevisionGeneral.length === 0 ? null : data.observacionesRevisionGeneral ) : null );
        this.addressForm.get('fecha').setValue(data.fechaExpedicion);
        this.addressForm.get('fechaAprob').setValue(data.fechaAprobacion);
        this.addressForm.get('cumpleAsegurado').setValue(data.cumpleDatosAsegurado);
        this.addressForm.get('cumpleBeneficiario').setValue(data.cumpleDatosBeneficiario);
        this.addressForm.get('cumpleAfianzado').setValue(data.cumpleDatosTomador);
        this.addressForm.get('reciboDePago').setValue(data.incluyeReciboPago);
        this.addressForm.get('condicionesGenerales').setValue(data.incluyeCondicionesGenerales);
        this.addressForm.get('vigenciaPoliza').setValue(data.vigencia);
        this.addressForm.get('vigenciaAmparo').setValue(data.vigenciaAmparo);
        this.addressForm.get('valorAmparo').setValue(data.valorAmparo);
        for (let i = 0; i < this.listaUsuarios.length; i++) {
          const responAprob = this.listaUsuarios.find(p => p.usuarioId === parseInt(data.responsableAprobacion));
          this.addressForm.get('responsableAprob').setValue(responAprob);
        }
        this.contrato = data;
        this.dataLoad2(data);
        if (data.nombreAseguradora){
          this.estaEditando = true;
          this.addressForm.markAllAsTouched();
        }
      });
    }

    dataLoad2(data) {
      this.idContrato = data.contratoId;
      this.idPoliza = data.contratoPolizaId;
      this.loadGarantia(this.idPoliza);
      this.loadObservations(this.idPoliza);
      this.loadEstadoRevision(this.idPoliza);
    }

    loadEstadoRevision(id) {
      this.polizaService.GetListPolizaObservacionByContratoPolizaId(id).subscribe(data => {
        for (let i = 0; i < data.length; i++) {
          const estadoRevSeleccionado = this.estadoArray.find(t => t.codigo === data[i].estadoRevisionCodigo);
          this.addressForm.get('fechaRevision').setValue(data[i].fechaRevision);
          this.addressForm.get('estadoRevision').setValue(estadoRevSeleccionado);
        }
      });
    }

    loadContratacionId(a) {
      this.contratacion.getContratacionByContratacionId(a).subscribe(data => {
        this.loadInfoContratacion(data);
      });
    }

    loadInfoContratacion(data) {
      this.nombreContratista = data.contratista.nombre;
      this.numeroIdentificacion = data.contratista.numeroIdentificacion;

    }

    loadObservations(id) {
      this.polizaService.GetListPolizaObservacionByContratoPolizaId(id).subscribe(data_1 => {
        this.polizaService.loadTableObservaciones.next(data_1);
        for (let i = 0; i < data_1.length; i++) {
          this.ultimoEstadoRevision = data_1[i].estadoRevisionCodigo;
          this.ultimaFechaRevision = data_1[i].fechaRevision;
        }
        const estadoRevisionCodigo = this.estadoArray.find(p => p.codigo === this.ultimoEstadoRevision);
        this.addressForm.get('fechaRevision').setValue(this.ultimaFechaRevision);
        this.addressForm.get('estadoRevision').setValue(estadoRevisionCodigo);
      });
    }

    loadGarantia(id) {
      this.polizaService.GetListPolizaGarantiaByContratoPolizaId(id).subscribe(data => {
        const tipoGarantiaCodigo = [];
        this.arrayGarantias = data;
        if (this.arrayGarantias.length > 0) {
          const polizasListRead = [this.arrayGarantias[0].tipoGarantiaCodigo];
          for (let i = 1; i < this.arrayGarantias.length; i++) {
            const Garantiaaux = polizasListRead.push(this.arrayGarantias[i].tipoGarantiaCodigo);
          }
          for (let i = 0; i < polizasListRead.length; i++) {
            const polizaSeleccionada = this.polizasYSegurosArray.filter(t => t.codigo === polizasListRead[i]);
            if (polizaSeleccionada.length > 0) { tipoGarantiaCodigo.push(polizaSeleccionada[0]) };
          }
          this.addressForm.get('polizasYSeguros').setValue(tipoGarantiaCodigo);
          for (let j = 0; j < polizasListRead.length; j++) {
            switch (polizasListRead[j]) {
              case '1':
                this.obj1 = true;
                this.addressForm.get('buenManejoCorrectaInversionAnticipo').setValue(this.arrayGarantias[j].esIncluidaPoliza);
                break;
              case '2':
                this.obj2 = true;
                this.addressForm.get('estabilidadYCalidad').setValue(this.arrayGarantias[j].esIncluidaPoliza);
                break;
              case '3':
                this.obj3 = true;
                this.addressForm.get('polizaYCoumplimiento').setValue(this.arrayGarantias[j].esIncluidaPoliza);
                break;
              case '4':
                this.obj4 = true;
                this.addressForm.get('polizasYSegurosCompleto').setValue(this.arrayGarantias[j].esIncluidaPoliza);
                break;
            }
          }
        };
      });
    }

    loadGrantiaID(id) {
      if (id != undefined) {
        this.idPoliza2 = id;
      }
      else {
        this.idPoliza2 = undefined;
      }
    }

    loadObservacionId2(id) {
      if (id != undefined) {
        this.idObservacion = id;
      }
      else {
        this.idObservacion = undefined;
      }
    }

    async getvalues( listCodigo: string[] ) {
        const listaSeguros = [ ...listCodigo ];

        if ( this.seguros.length > 0 ) {
            this.seguros.controls.forEach( ( control, indexControl ) => {
                const seguroIndex = listaSeguros.findIndex( codigo => codigo === control.get( 'tipoGarantiaCodigo' ).value );
                const seguro = listaSeguros.find( codigo => codigo === control.get( 'tipoGarantiaCodigo' ).value );

                if ( seguroIndex !== -1 ) {
                    listaSeguros.splice( seguroIndex, 1 );
                }

                if ( seguro === undefined ) {
                    this.seguros.removeAt( indexControl );
                    listaSeguros.splice( seguroIndex, 1 );
                }
            } );
        }

        for ( const codigo of listaSeguros ) {
            const seguro = this.polizasYSegurosArray.find( seguro => seguro.codigo === codigo );

            if ( seguro !== undefined ) {
                this.seguros.push( this.fb.group(
                    {
                        contratoPolizaId: [ 0 ],
                        polizaGarantiaId: [ 0 ],
                        nombre: [ seguro.nombre ],
                        tipoGarantiaCodigo: [ seguro.codigo ],
                        esIncluidaPoliza: [ null, Validators.required ],
                        vigencia: [ null, Validators.required ],
                        vigenciaAmparo: [ null, Validators.required ],
                        valorAmparo: [ null, Validators.required ]
                    }
                ) )
            }
        }
        console.log( this.seguros )
    }

    checkSeguros() {
        let registroCompletoSeguros = false;

        if ( this.seguros.length > 0 ) {
            let registroCompleto = 0;
            this.seguros.controls.forEach( control => {
                if ( control.get( 'esIncluidaPoliza' ).value !== null ) {
                    if ( control.get( 'esIncluidaPoliza' ).value === true ) {
                        if (control.get( 'vigencia' ).value !== null && control.get( 'vigenciaAmparo' ).value !== null && control.get( 'valorAmparo' ).value !== null ) {
                            registroCompleto++;
                        }
                    }

                    if ( control.get( 'esIncluidaPoliza' ).value === false ) {
                        registroCompleto++;
                    }
                }
            } );
            
            if ( registroCompleto === this.seguros.length ) {
                registroCompletoSeguros = true;
            }
        }

        return registroCompletoSeguros;
    }

    checkEsIncluidaPoliza( value: boolean, index: number ) {
        if ( value === false ) {
            this.seguros.controls[ index ].get( 'vigencia' ).setValue( null );
            this.seguros.controls[ index ].get( 'vigenciaAmparo' ).setValue( null );
            this.seguros.controls[ index ].get( 'valorAmparo' ).setValue( null );
        }
    }

    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    maxLength(e: any, n: number) {
      if (e.editor.getLength() > n) {
        e.editor.deleteText(n - 1, e.editor.getLength());
      }
    }

    textoLimpio( evento: any, n: number ) {
      if ( evento !== undefined ) {
          return evento.getLength() > n ? n : evento.getLength();
      } else {
          return 0;
      }
    }

    openDialog(modalTitle: string, modalText: string) {
      this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle, modalText }
      });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        let polizasList;
        const getPolizaGarantia = () => {
            const listSeguros = [];

            if ( this.seguros.length > 0 ) {
                this.seguros.controls.forEach( control => {
                    listSeguros.push(
                        {
                            contratoPolizaId: control.get( 'contratoPolizaId' ).value,
                            polizaGarantiaId: control.get( 'polizaGarantiaId' ).value,
                            tipoGarantiaCodigo: control.get( 'tipoGarantiaCodigo' ).value,
                            esIncluidaPoliza: control.get( 'esIncluidaPoliza' ).value,
                            vigencia: control.get( 'vigencia' ).value,
                            vigenciaAmparo: control.get( 'vigenciaAmparo' ).value,
                            valorAmparo: control.get( 'valorAmparo' ).value
                        }
                    )
                } )
            }

            return listSeguros.length > 0 ? listSeguros : null;
        }
        const getHistorialObservaciones = () => {
            return [
                {
                    polizaObservacionId: 0, 
                    contratoPolizaId: 0,
                    observacion: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : '',
                    fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : new Date().toISOString(),
                    estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value : this.estadosPoliza.enRevision
                }
            ]
        }

        const pContrato = {
            contratoId: Number( this.activatedRoute.snapshot.params.id ),
            contratoPoliza: [
                {
                    contratoId: Number( this.activatedRoute.snapshot.params.id ),
                    contratoPolizaId: 0,
                    nombreAseguradora: this.addressForm.get( 'nombre' ).value,
                    numeroPoliza: this.addressForm.get( 'numeroPoliza' ).value,
                    numeroCertificado: this.addressForm.get( 'numeroCertificado' ).value,
                    observacionesRevisionGeneral: this.addressForm.get( 'observacionesGenerales' ).value,
                    estadoPolizaCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? ( this.addressForm.get( 'estadoRevision' ).value === this.estadosPoliza.sinRadicacion ? this.estadosPoliza.polizaDevuelta : this.estadosPoliza.enRevision ) : this.estadosPoliza.enRevision,
                    fechaExpedicion: this.addressForm.get( 'fecha' ).value,
                    cumpleDatosAsegurado: this.addressForm.get( 'cumpleAsegurado' ).value,
                    cumpleDatosBeneficiario: this.addressForm.get( 'cumpleBeneficiario' ).value,
                    cumpleDatosTomador: this.addressForm.get( 'cumpleAfianzado' ).value,
                    incluyeReciboPago: this.addressForm.get( 'reciboDePago' ).value,
                    incluyeCondicionesGenerales: this.addressForm.get( 'condicionesGenerales' ).value,
                    fechaAprobacion: this.addressForm.get( 'fechaAprob' ).value,
                    polizaGarantia: getPolizaGarantia(),
                    historialObservaciones: getHistorialObservaciones()
                }
            ]
        }
    
        return;
        if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
          polizasList = [this.addressForm.value.polizasYSeguros[0].codigo];
          for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
            const membAux = polizasList.push(this.addressForm.value.polizasYSeguros[i].codigo);
          }
        }
        let nombreAprobado;
        if (this.addressForm.value.responsableAprob != undefined || this.addressForm.value.responsableAprob != null) {
          if (!this.addressForm.value.responsableAprob.usuarioId) {
            nombreAprobado = "pendiente";
          }
          else {
            nombreAprobado = this.addressForm.value.responsableAprob.usuarioId;
          }
        }
        var statePoliza = this.estadosPoliza.enRevision;
        let revEstado;
        if (this.addressForm.value.estadoRevision != undefined || this.addressForm.value.estadoRevision != null) {
          revEstado = this.addressForm.value.estadoRevision.codigo;
          if (revEstado == this.estadosPoliza.sinRadicacion ) {
            statePoliza = this.estadosPoliza.polizaDevuelta;
          }
          else {
            statePoliza = this.estadosPoliza.enRevision;
          }
        }
        var completo: boolean;
        if (this.addressForm.valid) {
          completo = true;
        }
        else {
          completo = false;
        }
    
        const contratoArray = {
          'contratoId': this.idContrato,
          "contratoPolizaId": this.idPoliza,
          'TipoSolicitudCodigo': "",
          'TipoModificacionCodigo': "",
          'DescripcionModificacion': "",
          'NombreAseguradora': this.addressForm.value.nombre,
          'NumeroPoliza': this.addressForm.value.numeroPoliza,
          'NumeroCertificado': this.addressForm.value.numeroCertificado,
          'Observaciones': "",
          'ObservacionesRevisionGeneral': this.addressForm.value.observacionesGenerales,
          'ResponsableAprobacion': nombreAprobado,
          'EstadoPolizaCodigo': statePoliza,
          //'UsuarioCreacion': "",
          //'UsuarioModificacion': "",
          'FechaExpedicion': this.addressForm.value.fecha,
          'Vigencia': this.addressForm.value.vigenciaPoliza,
          'VigenciaAmparo': this.addressForm.value.vigenciaAmparo,
          'ValorAmparo': this.addressForm.value.valorAmparo,
          'CumpleDatosAsegurado': this.addressForm.value.cumpleAsegurado,
          'CumpleDatosBeneficiario': this.addressForm.value.cumpleBeneficiario,
          'CumpleDatosTomador': this.addressForm.value.cumpleAfianzado,
          'IncluyeReciboPago': this.addressForm.value.reciboDePago,
          'IncluyeCondicionesGenerales': this.addressForm.value.condicionesGenerales,
          'FechaAprobacion': this.addressForm.value.fechaAprob,
          'Estado': false,
          //'FechaCreacion': "",
          'RegistroCompleto': completo,
          //'FechaModificacion': "",
          'Eliminado': false
        };
        const observacionArray = {
          polizaObservacionId: 0,
          contratoPolizaId: this.idPoliza,
          observacion: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : '',
          fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : null,
          estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value.codigo : null
        }
        let garantiaArray;
        if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
          for (let i = 0; i < polizasList.length; i++) {
            switch (polizasList[i]) {
              case '1':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '1',
                  'EsIncluidaPoliza': this.addressForm.value.buenManejoCorrectaInversionAnticipo
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
              case '2':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '2',
                  'EsIncluidaPoliza': this.addressForm.value.estabilidadYCalidad
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r1 => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
              case '3':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '3',
                  'EsIncluidaPoliza': this.addressForm.value.polizaYCoumplimiento
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r2 => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
              case '4':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '4',
                  'EsIncluidaPoliza': this.addressForm.value.polizasYSegurosCompleto
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r3 => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
            }
          }
        }
        this.polizaService.EditarContratoPoliza(contratoArray).subscribe(data => {
          if (data.isSuccessful == true) {
            /*this.polizaService.CreatePolizaGarantia(polizaGarantia).subscribe(data0=>{
    
            });*/
            this.polizaService.createEditPolizaObservacion( observacionArray )
              .subscribe(
                () => this.realizoPeticion = true,
                err => this.openDialog('', `<b>${err.message}</b>`)
              );
            /*
            this.polizaService.CambiarEstadoPolizaByContratoId(statePoliza, this.idContrato).subscribe(resp1 => {
    
            });
            */
            this.realizoPeticion = true;
            this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
            this.router.navigate(['/generarPolizasYGarantias']);
          }
          else {
            this.openDialog('', `<b>${data.message}</b>`);
          }
        }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
    }

}
