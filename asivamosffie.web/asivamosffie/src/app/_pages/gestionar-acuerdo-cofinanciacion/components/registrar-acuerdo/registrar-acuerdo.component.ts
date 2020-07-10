import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl, FormsModule } from '@angular/forms';
import { CofinanciacionService, CofinanciacionAportante, Cofinanciacion, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Dominio, CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { ClassGetter } from '@angular/compiler/src/output/output_ast';

import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-registrar-acuerdo',
  templateUrl: './registrar-acuerdo.component.html',
  styleUrls: ['./registrar-acuerdo.component.scss']
})
export class RegistrarAcuerdoComponent implements OnInit {

  mostrarDocumentosDeApropiacion = true;
  maxDate: Date;
  vigenciaEstados: number[];
  vigenciasAportante: number[];
  tiposDocumento: Dominio[]; 
  selectTiposAportante: Dominio[];
  nombresAportante: Dominio[];
  valorTotalAcuerdo = 85000000;
  listaCofinancAportantes: CofinanciacionAportante[] = [];
  selected = 2;

  constructor(private fb: FormBuilder,
              private cofinanciacionService: CofinanciacionService,
              private commonService: CommonService,
              public dialog: MatDialog,
              private activatedRoute: ActivatedRoute
              ) {
    this.maxDate = new Date();
  }

  datosAportantes = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    aportantes: this.fb.array([])
  });

  EditMode(){
    let id: number = 0;
    this.activatedRoute.params.subscribe( param =>{
    id = param['id'];

       if (id){
         this.cofinanciacionService.getAcuerdoCofinanciacionById(id).subscribe(cof => {
            this.datosAportantes.setControl('vigenciaEstado',this.fb.control(cof.vigenciaCofinanciacionId, Validators.required));
            this.datosAportantes.setControl('numAportes', this.fb.control(cof.cofinanciacionAportante.length));

            cof.cofinanciacionAportante.forEach( apor => {
              let grupo: FormGroup = this.createAportanteEditar(apor.tipoAportanteId, apor.nombreAportanteId, apor.cofinanciacionDocumento.length);   

              const valorTipo = this.selectTiposAportante.find( a => a.dominioId == apor.tipoAportanteId);
              const valorNombre = this.nombresAportante.find( a => a.dominioId == apor.nombreAportanteId);

              console.log(cof);
              
              grupo.get('tipo').setValue(valorTipo);
              grupo.get('nombre').setValue(valorNombre);

              this.aportantes.push( grupo );
            });
  
            this.listaCofinancAportantes = cof.cofinanciacionAportante;
            this.listaAportantes();

           });
       }
    });
  }

  ngOnInit(): void {

    this.vigenciasAportante = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    this.vigenciaEstados = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    
    forkJoin
    ([
      this.commonService.listaTipoDocFinanciacion(),
      this.commonService.listaTipoAportante(),
      this.commonService.listaNombreAportante(),
    ]).subscribe
      (
        res =>
        {
          this.tiposDocumento = res[0];
          this.selectTiposAportante = res[1];
          this.nombresAportante = res[2];
          this.EditMode();
        }
      )
  }

  get aportantes() {
    return this.datosAportantes.get('aportantes') as FormArray;
  }

  get DocumentoAportantes() {
    return this.documentoApropiacion;
  }

  // tabla de los documentos de aportantes
  documentoApropiacion:CofinanciacionDocumento[]=[];

  CambioNumeroAportantes(){
    const FormNumAportantes = this.datosAportantes.value;
    if (FormNumAportantes.numAportes > this.aportantes.length && FormNumAportantes.numAportes < 100) {
      while (this.aportantes.length < FormNumAportantes.numAportes) {
        this.aportantes.push( this.createAportante() );
        //this.DocumentoAportantes.push( this.createDocumentoAportante() );
      }
    } else if (FormNumAportantes.numAportes <= this.aportantes.length && FormNumAportantes.numAportes >= 0) {
      while (this.aportantes.length > FormNumAportantes.numAportes) {
        this.borrarAportante(this.aportantes, this.aportantes.length - 1);
        this.borrarAportante(this.DocumentoAportantes, this.aportantes.length - 1);
      }
    }
  }

  createAportante(): FormGroup {
    return this.fb.group({
      tipo: ['', Validators.required],
      nombre: ['', Validators.required],
      cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    });
  }

  createAportanteEditar(pTipo: number, pNombre: number, pCantidad: number): FormGroup {
    return this.fb.group({
      tipo: [pTipo, Validators.required],
      nombre: [pNombre, Validators.required],
      cauntosDocumentos: [pCantidad, [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    });
  }

  createDocumentoAportante(): FormGroup {
    return this.fb.group({
      vigenciaAportante: [null, Validators.required],
      valorIndicadoEnElDocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      tipoDocumento: [null, Validators.required],
      numeroDocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(10), Validators.maxLength(10)])
      ],
      fechaDocumento: [null, Validators.required]
    });
  }


  borrarAportante(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  listaAportantes()
  {
    //this.listaCofinancAportantes=[];
    let i =0;
    let listaCofinancAportant:CofinanciacionAportante[] = [];
    this.aportantes.controls.forEach( control => 
      {
        let cofinanciacionDocumento=[]
        if(this.listaCofinancAportantes[i])
        {
          if(this.listaCofinancAportantes[i].cofinanciacionDocumento.length>0)
          {
            cofinanciacionDocumento=this.listaCofinancAportantes[i].cofinanciacionDocumento;
          }    
        }
        let cofiApo: CofinanciacionAportante={
          tipoAportanteId:control.get('tipo').value,
          nombreAportanteId:control.get('nombre').value,municipioId:null,cofinanciacionId:null,
          cofinanciacionAportanteId:null,cofinanciacionDocumento:cofinanciacionDocumento
        }

        //cofiApo.cofinanciacionDocumento = this.listaDocumentos( control )
                
        listaCofinancAportant.push(cofiApo);
        i++;
      });    
      this.listaCofinancAportantes=listaCofinancAportant;  
  }

  onSubmit() {
    console.log("entró");
    if (this.datosAportantes.valid) {

      let cofinanciacion: Cofinanciacion = 
      {
        vigenciaCofinanciacionId: this.datosAportantes.get('vigenciaEstado').value,
        cofinanciacionAportante: this.listaCofinancAportantes,
        cofinanciacionId:0
      }

      console.log(cofinanciacion);

      cofinanciacion.cofinanciacionAportante.forEach(apo =>
        {
          apo.tipoAportanteId = apo.tipoAportanteId.dominioId;
          apo.nombreAportanteId = apo.nombreAportanteId.dominioId;
        });

      this.cofinanciacionService.CrearOModificarAcuerdoCofinanciacion(cofinanciacion).subscribe( 
        respuesta => 
        {
          this.verificarRespuesta( respuesta );
        },
        err => {
          let mensaje: string;
          if (err.error.message){
            mensaje = err.error.message;
          }else {
            mensaje = err.message;
          }
          this.openDialog('Error', mensaje);
       },
       () => {
        //console.log('terminó');
       });

      this.mostrarDocumentosDeApropiacion = true;
      //genero los documentos de manera dinamica
      this.aportantes.controls.forEach( control => 
        {
          let cantidad=control.get('cauntosDocumentos').value;
          for(let a=0;a<cantidad;a++)
          {
            //this.DocumentoAportantes.push( this.createDocumentoAportante());
          }
        });
    }
    
    
  }

  private verificarRespuesta( respuesta: Respuesta )
  {
    if (respuesta.isSuccessful) // Response witout errors
    {
      this.openDialog('', respuesta.message);
      if (respuesta.isValidation) // have validations
      {
        
      }
     }else{
      this.openDialog('', respuesta.message);
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  cantidadDocumentos(data:any,identificador:number)
  {
    //this.documentoApropiacion=[];
    this.listaAportantes();
    
    let aportante = this.listaCofinancAportantes.filter(function(element) {
      return element.tipoAportanteId == data.value.tipo && element.nombreAportanteId==data.value.nombre;
    });
    
    const index = this.listaCofinancAportantes.indexOf(aportante[0], 0);
    
    this.listaCofinancAportantes[index].cofinanciacionDocumento=[];
    for(let i=0;i<data.value.cauntosDocumentos;i++)
    {
      this.listaCofinancAportantes[index].cofinanciacionDocumento.push({cofinanciacionAportanteId:identificador,fechaAcuerdo:null,cofinanciacionDocumentoId:null,
        numeroActa:null,tipoDocumentoId:null,valorDocumento:null,valorTotalAportante:null,vigenciaAporte:null});
    }
    
  }
}
