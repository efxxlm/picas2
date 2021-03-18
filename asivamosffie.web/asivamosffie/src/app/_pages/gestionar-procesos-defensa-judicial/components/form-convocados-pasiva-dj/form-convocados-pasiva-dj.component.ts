import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
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
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
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
      } )
  };

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

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroContratos: `${ this.perfiles.length }`
    });
  };

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
