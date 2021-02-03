import { Component, Input, OnInit,AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService, DemandadoConvocado, DemandanteConvocante } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-demandantes-convocantes-dj',
  templateUrl: './form-demandantes-convocantes-dj.component.html',
  styleUrls: ['./form-demandantes-convocantes-dj.component.scss']
})
export class FormDemandantesConvocantesDjComponent implements OnInit {
  addressForm = this.fb.group({
    demandaContraFFIE: [null, Validators.required]
  });
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

  textoConvocantes="demandante";

  constructor (private fb: FormBuilder,public commonService:CommonService,
    public defensaService:DefensaJudicialService,
    public dialog: MatDialog, private router: Router  ) {
    this.crearFormulario();
  }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;
  ngAfterViewInit(){
    this.cargarRegistro();
  }
  cargarRegistro() {
     

/*      let defContraProyecto:DemandanteConvocante[]=[];
    for(let perfil of this.perfiles.controls){
      defContraProyecto.push({
        nombre:perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
        numeroIdentificacion:perfil.get("numIdentificacion").value,
        direccion:perfil.get("direccion").value,
        email:perfil.get("correo").value,
        esConvocante:false
      });
    };
*/
    this.addressForm.get("demandaContraFFIE").setValue(this.defensaJudicial.esDemandaFfie);
    this.formContratista.get("numeroContratos").setValue(this.defensaJudicial.numeroDemandantes);
    let i=0;
    console.log(this.perfiles);
    this.defensaJudicial.demandadoConvocado.forEach(element => {
      console.log(this.perfiles.controls[i].get("nomConvocado"));
      this.perfiles.controls[i].get("nomConvocado").setValue(element.nombre);
      this.perfiles.controls[i].get("tipoIdentificacion").setValue(element.tipoIdentificacionCodigo);
      this.perfiles.controls[i].get("numIdentificacion").setValue(element.numeroIdentificacion);
      this.perfiles.controls[i].get("direccion").setValue(element.direccion);
      this.perfiles.controls[i].get("correo").setValue(element.email);
      i++;
    });
    //defensaJudicial.eDemandaFFIE=this.addressForm.get("demandaContraFFIE").value;
    //=;
  }

  ngOnInit(): void {
    this.commonService.listaTipodocumento().subscribe(response=>{
      this.tiposIdentificacionArray=response;
    });
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        console.log(this.perfiles.length);
        console.log(value);
        if(this.perfiles.length>Number(value))
        {
          //verifico si tiene datos para mandar la alerta
          this.perfiles.value.forEach(element => {
            if(element.nomConvocado!= null || element.correo!= null ||
              element.direccion!=null || element.numIdentificacion != null ||
              element.tipoIdentificacion!=null)
              {
                this.openDialog("","<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>");
              }
              else
              {
                this.perfiles.removeAt(this.perfiles.length-1);
              }
          });
        }
        else
        {
          for ( let i = this.perfiles.length; i < Number(value); i++ ) {
            this.perfiles.push( 
              this.fb.group(
                {
                  nomConvocado: [ null ],
                  tipoIdentificacion: [ null ],
                  numIdentificacion: [ null ],
                  direccion: [ null ],
                  correo: [ null ]
                }
              ) 
            )
          }
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
    let defContraProyecto:DemandanteConvocante[]=[];
    for(let perfil of this.perfiles.controls){
      defContraProyecto.push({
        nombre:perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
        numeroIdentificacion:perfil.get("numIdentificacion").value,
        direccion:perfil.get("direccion").value,
        email:perfil.get("correo").value,
        esConvocante:false
      });
    };
    
    let defensaJudicial=this.defensaJudicial;
    if(!this.defensaJudicial.defensaJudicialId||this.defensaJudicial.defensaJudicialId==0)
    {
      defensaJudicial={
        defensaJudicialId:this.defensaJudicial.defensaJudicialId,
        tipoProcesoCodigo:this.tipoProceso,
        esLegitimacionActiva:this.legitimacion,
      };
    }
    defensaJudicial.esDemandaFfie=this.addressForm.get("demandaContraFFIE").value;
    defensaJudicial.numeroDemandantes=this.formContratista.get("numeroContratos").value;

    defensaJudicial.demandanteConvocante=defContraProyecto;
    
      console.log(defensaJudicial);
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
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        if(id>0 && this.defensaJudicial.defensaJudicialId==0)
        {
          this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/"+id], {});
        }                  
        else{
          location.reload();
        }                   
      });
    }
  
  }

  cambioTipoTexto()
  {    
    this.textoConvocantes=this.addressForm.value.demandaContraFFIE?"demandante":"convocante";
  }

}
