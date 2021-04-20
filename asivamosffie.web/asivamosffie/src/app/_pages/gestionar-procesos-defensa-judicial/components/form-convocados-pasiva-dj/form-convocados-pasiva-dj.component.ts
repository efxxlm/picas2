import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService, DemandadoConvocado } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-convocados-pasiva-dj',
  templateUrl: './form-convocados-pasiva-dj.component.html',
  styleUrls: ['./form-convocados-pasiva-dj.component.scss']
})
export class FormConvocadosPasivaDjComponent implements OnInit {
  formContratista: FormGroup;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  tiposIdentificacionArray = [
  ];
  departamentoArray = [
  ];
  municipioArray = [
  ];
  tipoAccionArray = [
  ];
  jurisdiccionArray = [
  ];
  intanciasArray = [
  ];
  estaEditando = false;
  constructor ( private fb: FormBuilder,public commonService:CommonService,
    public defensaService:DefensaJudicialService,
    public dialog: MatDialog, private router: Router  ) {
    this.crearFormulario();
    this.getNumeroContratos();
  }

  @Input() legitimacion:boolean;
  @Input() demandaContraFFIE:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;

  ngAfterViewInit(){
    this.cargarRegistro();
  }
  cargarRegistro() {
    console.log(this.defensaJudicial.numeroDemandados);
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    this.formContratista.get("numeroContratos").setValue(this.defensaJudicial.numeroDemandados);
      let i=0; 

      let listaConvocados:DemandadoConvocado[]= [];

      this.defensaJudicial.demandadoConvocado.forEach(element => {
        if (element.esConvocado == true)
        listaConvocados.push(element);
      });

      listaConvocados.forEach(element => {
          console.log(this.perfiles.controls[i].get("nomConvocado"));
          this.perfiles.controls[i].markAllAsTouched();
          this.perfiles.controls[i].get("demandadoConvocadoId").setValue(element.demandadoConvocadoId);
          this.perfiles.controls[i].get("nomConvocado").setValue(element.nombre);
          this.perfiles.controls[i].get("tipoIdentificacion").setValue(element.tipoIdentificacionCodigo);
          this.perfiles.controls[i].get("numIdentificacion").setValue(element.numeroIdentificacion);
          this.perfiles.controls[i].get("conocimientoParteAutoridad").setValue(element.existeConocimiento);
          this.perfiles.controls[i].get("despacho").setValue(element.convocadoAutoridadDespacho);
          if(element.localizacionIdMunicipio != null){
            this.commonService.listMunicipiosByIdMunicipio(element.localizacionIdMunicipio.toString()).subscribe(res=>{
              this.perfiles.controls[i].get("departamento").setValue(res[0].idPadre);
              this.municipioArray=res;
              this.perfiles.controls[i].get("municipio").setValue(element.localizacionIdMunicipio);
            });
          }
          this.perfiles.controls[i].get("radicadoDespacho").setValue(element.radicadoDespacho);
          this.perfiles.controls[i].get("fechaRadicadoDespacho").setValue(element.fechaRadicado);
          this.perfiles.controls[i].get("accionAEvitar").setValue(element.medioControlAccion);
          this.perfiles.controls[i].get("etapaProcesoFFIE").setValue(element.etapaProcesoFfiecodigo);
          this.perfiles.controls[i].get("caducidad").setValue(element.caducidadPrescripcion);
          //this.perfiles.controls[i].get("registroCompleto").setValue(element.registroCompleto);
          if( element.registroCompleto == null 
            || (!element.registroCompleto 
            && (element.nombre == null || element.nombre == '')
            && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
            && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
            && (element.existeConocimiento == null) 
            )){
              this.perfiles.controls[i].get("registroCompleto").setValue(null);
            }else if(!element.registroCompleto){
              this.perfiles.controls[i].get("registroCompleto").setValue(false);
            }else if(element.registroCompleto){
              this.perfiles.controls[i].get("registroCompleto").setValue(true);
            }
        i++;
      });     
  }

  ngOnInit(): void {
    this.commonService.listaTipodocumento().subscribe(response=>{
      this.tiposIdentificacionArray=response;
    });
    this.commonService.listaDepartamentos().subscribe(response=>{
      this.departamentoArray=response;
    });
    this.commonService.listaTipoAccionJudicial().subscribe(response=>{
      this.tipoAccionArray=response;
    });
    this.commonService.listaEtapaJudicial().subscribe(response=>{
      this.intanciasArray=response;
    });
    this.formContratista.get('numeroContratos').valueChanges
    .subscribe(value => {
      if (this.perfiles.length < Number(value)) {
        for (let i = this.perfiles.length; i < Number(value); i++) {
          this.perfiles.push(
            this.fb.group(
              {
                demandadoConvocadoId: [ null ],
                nomConvocado: [ null ],
                tipoIdentificacion: [ null ],
                numIdentificacion: [ null ],
                conocimientoParteAutoridad: [ null ],
                despacho: [ null ],
                departamento: [ null ],
                municipio: [ null ],
                radicadoDespacho: [ null ],
                fechaRadicadoDespacho: [ null ],
                accionAEvitar: [ null ],
                etapaProcesoFFIE: [ null ],
                caducidad: [ null ],
                registroCompleto: [ null ],
              }
            )
          )
        }
      }
    });
  };

  getNumeroContratos() {
    this.formContratista.get( 'numeroContratos' ).valueChanges
        .subscribe(
            value => {
                if ( this.defensaJudicial !== undefined && this.defensaJudicial.demandadoConvocado.length > 0 ) {
                    this.perfiles.clear();
                    for (const demandadoConvocado of this.defensaJudicial.demandadoConvocado ) {
                      let departamentoD = "";
                      let municipioD = "";

                      if(demandadoConvocado.localizacionIdMunicipio != null){
                        this.commonService.listMunicipiosByIdMunicipio(demandadoConvocado.localizacionIdMunicipio.toString()).subscribe(res=>{
                          departamentoD = res[0].idPadre;
                          municipioD = "";
                        });
                      }  
                      this.perfiles.push(
                            this.fb.group(
                                {
                                    demandadoConvocadoId: demandadoConvocado.demandadoConvocadoId,
                                    nomConvocado: demandadoConvocado.nombre !== undefined ? demandadoConvocado.nombre : null,
                                    tipoIdentificacion: demandadoConvocado.tipoIdentificacionCodigo !== undefined ? demandadoConvocado.tipoIdentificacionCodigo : null,
                                    numIdentificacion: demandadoConvocado.numeroIdentificacion !== undefined ? demandadoConvocado.numeroIdentificacion : null,
                                    conocimientoParteAutoridad: demandadoConvocado.existeConocimiento,
                                    despacho: demandadoConvocado.convocadoAutoridadDespacho,
                                    departamento: departamentoD !== undefined ? departamentoD : null,
                                    municipio:municipioD !== undefined ? municipioD : null,
                                    radicadoDespacho: demandadoConvocado.radicadoDespacho !== undefined ? demandadoConvocado.radicadoDespacho : null,
                                    fechaRadicadoDespacho: demandadoConvocado.fechaRadicado !== undefined ? demandadoConvocado.fechaRadicado : null,
                                    accionAEvitar: demandadoConvocado.medioControlAccion !== undefined ? demandadoConvocado.medioControlAccion : null,
                                    etapaProcesoFFIE: demandadoConvocado.etapaProcesoFfiecodigo !== undefined ? demandadoConvocado.etapaProcesoFfiecodigo : null,
                                    caducidad: demandadoConvocado.caducidadPrescripcion !== undefined ? demandadoConvocado.caducidadPrescripcion : null,
                                    registroCompleto: demandadoConvocado.registroCompleto !== undefined ? demandadoConvocado.registroCompleto : null,
                                  }
                            )
                        );
                    }
                    this.formContratista.get( 'numeroContratos' ).setValidators( Validators.min( this.perfiles.length ) );
                    const nuevosConvocados = Number( value ) - this.perfiles.length;
                    if ( Number( value ) < this.perfiles.length && Number( value ) > 0 ) {
                      console.log("1");
                      this.openDialog(
                        '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                      );
                      this.formContratista.get( 'numeroContratos' ).setValue( String( this.perfiles.length ) );
                      return;
                    }
                    for ( let i = 0; i < nuevosConvocados; i++ ) {
                        this.perfiles.push(
                            this.fb.group({
                              demandadoConvocadoId: [ null ],
                              nomConvocado: [ null ],
                              tipoIdentificacion: [ null ],
                              numIdentificacion: [ null ],
                              conocimientoParteAutoridad: [ null ],
                              despacho: [ null ],
                              departamento: [ null ],
                              municipio: [ null ],
                              radicadoDespacho: [ null ],
                              fechaRadicadoDespacho: [ null ],
                              accionAEvitar: [ null ],
                              etapaProcesoFFIE: [ null ],
                              caducidad: [ null ],
                              registroCompleto: [ null ],
                            })
                        );
                    }
                }else if (this.defensaJudicial !== undefined && this.defensaJudicial.demandadoConvocado.length === 0 )
                {
                    if ( Number( value ) < 0 ) {
                        this.formContratista.get( 'numeroContratos' ).setValue( '0' );
                    }
                    if ( Number( value ) > 0 ) {
                        if ( this.formContratista.dirty === true ) {
                            this.formContratista.get( 'numeroContratos' )
                            .setValidators( Validators.min( this.perfiles.length ) );
                            const nuevosConvocados = Number( value ) - this.perfiles.length;
                            if ( Number( value ) < this.perfiles.length && Number( value ) > 0 ) {
                              console.log("2");
                              this.openDialog(
                                '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                              );
                              this.formContratista.get( 'numeroContratos' ).setValue( String( this.perfiles.length ) );
                              return;
                            }
                            for ( let i = 0; i < nuevosConvocados; i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      conocimientoParteAutoridad: [ null ],
                                      despacho: [ null ],
                                      departamento: [ null ],
                                      municipio: [ null ],
                                      radicadoDespacho: [ null ],
                                      fechaRadicadoDespacho: [ null ],
                                      accionAEvitar: [ null ],
                                      etapaProcesoFFIE: [ null ],
                                      caducidad: [ null ],
                                      registroCompleto: [ null ],
                                    })
                                );
                            }
                        } else {
                            this.perfiles.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      conocimientoParteAutoridad: [ null ],
                                      despacho: [ null ],
                                      departamento: [ null ],
                                      municipio: [ null ],
                                      radicadoDespacho: [ null ],
                                      fechaRadicadoDespacho: [ null ],
                                      accionAEvitar: [ null ],
                                      etapaProcesoFFIE: [ null ],
                                      caducidad: [ null ],
                                      registroCompleto: [ null ],
                                    })
                                );
                            }
                        }
                    }
                }else if ( this.defensaJudicial === undefined ) {
                    if ( Number( value ) < 0 ) {
                        this.formContratista.get( 'numeroContratos' ).setValue( '0' );
                    }
                    if ( Number( value ) > 0 ) {
                        if ( this.perfiles.dirty === true ) {
                            this.formContratista.get( 'numeroContratos' )
                            .setValidators( Validators.min( this.perfiles.length ) );
                            const nuevosConvocados = Number( value ) - this.perfiles.length;
                            if ( Number( value ) < this.perfiles.length && Number( value ) > 0 ) {
                              console.log("3");
                              this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                              this.formContratista.get( 'numeroContratos' ).setValue( String( this.perfiles.length ) );
                              return;
                            }
                            for ( let i = 0; i < nuevosConvocados; i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      conocimientoParteAutoridad: [ null ],
                                      despacho: [ null ],
                                      departamento: [ null ],
                                      municipio: [ null ],
                                      radicadoDespacho: [ null ],
                                      fechaRadicadoDespacho: [ null ],
                                      accionAEvitar: [ null ],
                                      etapaProcesoFFIE: [ null ],
                                      caducidad: [ null ],
                                      registroCompleto: [ null ],
                                    })
                                );
                            }
                        } else {
                            this.perfiles.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.perfiles.push(
                                    this.fb.group({
                                      demandadoConvocadoId: [ null ],
                                      nomConvocado: [ null ],
                                      tipoIdentificacion: [ null ],
                                      numIdentificacion: [ null ],
                                      conocimientoParteAutoridad: [ null ],
                                      despacho: [ null ],
                                      departamento: [ null ],
                                      municipio: [ null ],
                                      radicadoDespacho: [ null ],
                                      fechaRadicadoDespacho: [ null ],
                                      accionAEvitar: [ null ],
                                      etapaProcesoFFIE: [ null ],
                                      caducidad: [ null ],
                                      registroCompleto: [ null ],
                                    })
                                );
                            }
                        }
                    }
                }
            }
        );
  }

	  get perfiles () {
		return this.formContratista.get( 'perfiles' ) as FormArray;
	  };

  get numeroRadicado () {
    let numero;
    Object.values( this.formContratista.controls ).forEach( control => {
      if ( control instanceof FormArray ) {
        Object.values( control.controls ).forEach( control => {
          numero = control.get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
        } )
      }
    } )
    return numero;
  };
  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };
  textoLimpioNew(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };
  
  openDialogSiNo(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    return dialogRef.afterClosed();
  }

  eliminarPerfil( demandadoConvocadoId: number, numeroPerfil: number ) {
    this.openDialogSiNo( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value === true ) {
            if ( demandadoConvocadoId === 0 || demandadoConvocadoId == null) {
                this.perfiles.removeAt( numeroPerfil );
                this.formContratista.patchValue({
                  numeroContratos: `${ this.perfiles.length }`
                });
                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
            } else {
              this.perfiles.removeAt( numeroPerfil );
              this.formContratista.patchValue({
                numeroContratos: `${ this.perfiles.length }`
              });
                this.defensaService.deleteDemandadoConvocado( demandadoConvocadoId , this.perfiles.length)
                    .subscribe(
                        response => {
                            this.openDialog( '', `<b>${ response.message }</b>` );
                            this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                () =>   this.router.navigate(
                                            [
                                                '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                                                this.defensaJudicial.defensaJudicialId
                                            ]
                                        )
                            );
                        },
                        err => this.openDialog( '', `<b>${ err.message }</b>` )
                    );
            }
        }
      } );
}

  agregarNumeroRadicado () {
    this.numeroRadicado.push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado.removeAt( numeroRadicado );
  };

  guardar () {
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    this.perfiles.markAllAsTouched();
    console.log( this.formContratista );
    let defContraProyecto:DemandadoConvocado[]=[];
    for(let perfil of this.perfiles.controls){
      defContraProyecto.push({
        demandadoConvocadoId:perfil.get("demandadoConvocadoId").value,
        nombre:perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
        numeroIdentificacion:perfil.get("numIdentificacion").value,
        existeConocimiento:perfil.get("conocimientoParteAutoridad").value,
        convocadoAutoridadDespacho:perfil.get("despacho").value,
        localizacionIdMunicipio:perfil.get("municipio").value,
        radicadoDespacho:perfil.get("radicadoDespacho").value,
        fechaRadicado:perfil.get("fechaRadicadoDespacho").value,
        medioControlAccion:perfil.get("accionAEvitar").value,
        etapaProcesoFfiecodigo:perfil.get("etapaProcesoFFIE").value,
        caducidadPrescripcion:perfil.get("caducidad").value,
        esConvocado:true//lo es para este modulo
      });
    };
    
    let defensaJudicial=this.defensaJudicial;
    if(!this.defensaJudicial.defensaJudicialId||this.defensaJudicial.defensaJudicialId==0)
    {
      defensaJudicial={
        defensaJudicialId:this.defensaJudicial.defensaJudicialId,
        //legitimacionCodigo:this.legitimacion,
        tipoProcesoCodigo:this.tipoProceso,
        //cantContratos:this.formContratista.get( 'numeroContratos' ).value,
        esLegitimacionActiva:this.legitimacion,
        esCompleto:false,      
      };
    }else{
      this.tipoProceso != null ? defensaJudicial.tipoProcesoCodigo = this.tipoProceso : this.defensaJudicial.tipoProcesoCodigo;
      this.legitimacion != null ? defensaJudicial.esLegitimacionActiva = this.legitimacion : this.defensaJudicial.esLegitimacionActiva;
    }
    console.log(this.defensaJudicial, " - ", this.tipoProceso, " - ", this.legitimacion);
    this.demandaContraFFIE != null ? defensaJudicial.esDemandaFfie = this.demandaContraFFIE : "";
    defensaJudicial.numeroDemandados=this.formContratista.get("numeroContratos").value;
    defensaJudicial.demandadoConvocado=defContraProyecto;
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        if (id > 0 && this.defensaJudicial.defensaJudicialId != id) {
          this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () =>   this.router.navigate(
                        [
                            '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                            id
                        ]
                    )
          );
        }
        else {
          if(this.defensaJudicial.defensaJudicialId == id){
            this.router.navigateByUrl( '/', {skipLocationChange: true} ).then(
              () =>   this.router.navigate(
                          [
                              '/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',
                              id
                          ]
                      )
            );
          }else{
            this.router.navigate(["/gestionarProcesoDefensaJudicial"], {});
          }
        }
      });
    }
  }

  changeDepartamento(id: string | number) {
    console.log(this.perfiles.controls[id]);
    this.commonService.listaMunicipiosByIdDepartamento(this.perfiles.controls[id]
      .get('departamento').value).subscribe(mun => {
        this.municipioArray=mun;
        //this.perfiles.controls[id].get('municipios').setValue(mun);
      });
  }  

}
