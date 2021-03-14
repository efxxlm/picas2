import { Component, Input, OnInit, SimpleChange, SimpleChanges, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DefensaJudicial } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { CommonService, Respuesta } from '../../../../core/_services/common/common.service';
import { DefensaJudicialService, DemandadoConvocado } from '../../../../core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-convocados-dj',
  templateUrl: './form-convocados-dj.component.html',
  styleUrls: ['./form-convocados-dj.component.scss']
})
export class FormConvocadosDjComponent implements OnInit {
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

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;
  estaEditando = false;
  cargarRegistro() {
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    this.formContratista.get("numeroContratos").setValue(this.defensaJudicial.numeroDemandados);
      let i=0; 

      this.defensaJudicial.demandadoConvocado.forEach(element => {
        this.perfiles.controls[i].markAllAsTouched();
        this.perfiles.controls[i].get("demandadoConvocadoId").setValue(element.demandadoConvocadoId);
        this.perfiles.controls[i].get("nomConvocado").setValue(element.nombre);
        this.perfiles.controls[i].get("tipoIdentificacion").setValue(element.tipoIdentificacionCodigo);
        this.perfiles.controls[i].get("numIdentificacion").setValue(element.numeroIdentificacion);
        this.perfiles.controls[i].get("direccion").setValue(element.direccion);
        this.perfiles.controls[i].get("correo").setValue(element.email);
        //this.perfiles.controls[i].get("registroCompleto").setValue(element.registroCompleto);
        if( element.registroCompleto == null 
          || (!element.registroCompleto 
          && (element.nombre == null || element.nombre == '')
          && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
          && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
          && (element.direccion == null || element.direccion == '') 
          && (element.email == null || element.email == '') 
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

  constructor ( private fb: FormBuilder,public commonService:CommonService,
    public defensaService:DefensaJudicialService,
    public dialog: MatDialog, private router: Router  ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.commonService.listaTipodocumento().subscribe(response=>{
      this.tiposIdentificacionArray=response;
    });
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                registroCompleto: [ null ],
                demandadoConvocadoId: [ null ],
                nomConvocado: [ null ],
                tipoIdentificacion: [ null ],
                numIdentificacion: [ null ],
                direccion: [ null ],
                correo: [ null ]
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
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };
  openDialogSiNo(modalTitle: string, modalText: string,id) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.eliminarPerfil(id); 
      }           
    });
  }
  eliminar(id)
  {
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",id);
  }
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
    // console.log( this.formContratista );
    let defContraProyecto:DemandadoConvocado[]=[];
    for(let perfil of this.perfiles.controls){
      defContraProyecto.push({
        demandadoConvocadoId: perfil.get("demandadoConvocadoId").value,
        nombre:perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
        numeroIdentificacion:perfil.get("numIdentificacion").value,
        direccion:perfil.get("direccion").value,
        email:perfil.get("correo").value,
        esConvocado:false //para este modulo, no lo es
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
    defensaJudicial.demandadoConvocado=defContraProyecto;
    defensaJudicial.numeroDemandados=this.formContratista.get("numeroContratos").value;
    if(this.tipoProceso==null || this.legitimacion==null){
      this.openDialog('', '<b>Falta registrar información.</b>');
    }
    else{
        this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial)
        .subscribe((response: Respuesta) => {
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        },
        err => {
          this.openDialog('', err.message);
        });
    }
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        if (id > 0 && this.defensaJudicial.defensaJudicialId != id) {
          this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/" + id], {});
        }
        else {
          if(this.defensaJudicial.defensaJudicialId == id){
            location.reload();
          }else{
            this.router.navigate(["/gestionarProcesoDefensaJudicial"], {});
          }
        }
      });
    }
  }
  
}
