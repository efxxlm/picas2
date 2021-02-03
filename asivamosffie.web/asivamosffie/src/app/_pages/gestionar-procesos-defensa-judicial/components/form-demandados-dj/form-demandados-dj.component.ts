import { Component, Input, OnInit,AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService, DemandadoConvocado } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-demandados-dj',
  templateUrl: './form-demandados-dj.component.html',
  styleUrls: ['./form-demandados-dj.component.scss']
})
export class FormDemandadosDjComponent implements OnInit {
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

  constructor ( private fb: FormBuilder,public commonService:CommonService,
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
      this.formContratista.get("numeroContratos").setValue(this.defensaJudicial.numeroDemandados);
      let i=0;      
      this.defensaJudicial.demandanteConvocante.forEach(element => {
        console.log(this.perfiles.controls[i].get("nomConvocado"));
        this.perfiles.controls[i].get("nomConvocado").setValue(element.nombre);
        this.perfiles.controls[i].get("tipoIdentificacion").setValue(element.tipoIdentificacionCodigo);
        this.perfiles.controls[i].get("numIdentificacion").setValue(element.numeroIdentificacion);
        
        i++;
      });
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
                nomConvocado: [ null ],
                tipoIdentificacion: [ null ],
                numIdentificacion: [ null ]
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
    console.log( this.formContratista );
    let defContraProyecto:DemandadoConvocado[]=[];
    for(let perfil of this.perfiles.controls){
      defContraProyecto.push({
        nombre:perfil.get("nomConvocado").value,
        tipoIdentificacionCodigo:perfil.get("tipoIdentificacion").value,
        numeroIdentificacion:perfil.get("numIdentificacion").value,        
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
    }
    defensaJudicial.numeroDemandados=this.formContratista.get("numeroContratos").value;
    defensaJudicial.demandadoConvocado=defContraProyecto;
    
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
          if(id>0)
          {
            this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/"+id], {});
          }                  
      });
    }
  }

}
