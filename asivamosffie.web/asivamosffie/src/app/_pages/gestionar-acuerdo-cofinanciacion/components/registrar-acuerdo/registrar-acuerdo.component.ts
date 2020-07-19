import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl, FormsModule, ControlContainer } from '@angular/forms';
import { CofinanciacionService, CofinanciacionAportante, Cofinanciacion, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Dominio, CommonService, Respuesta, Localizacion } from 'src/app/core/_services/common/common.service';
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
  departamentos: Localizacion[];
  valorTotalAcuerdo = 0;
  listaCofinancAportantes: CofinanciacionAportante[] = [];
  id: number = 0;
  tiposPersonaHabilitaNombre: string[] = ['3'];

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
    this.activatedRoute.params.subscribe( param =>{
    

       if (param['id']){
        this.id = param['id'];
        this.cofinanciacionService.getAcuerdoCofinanciacionById(this.id).subscribe(cof => {
            this.datosAportantes.setControl('vigenciaEstado',this.fb.control(cof.vigenciaCofinanciacionId, Validators.required));
            this.datosAportantes.setControl('numAportes', this.fb.control(cof.cofinanciacionAportante.length));

            cof.cofinanciacionAportante.forEach( apor => {
              let grupo: FormGroup = this.createAportanteEditar(apor.tipoAportanteId, apor.nombreAportanteId, apor.cofinanciacionDocumento.length,
                                                                apor.cofinanciacionId, apor.cofinanciacionAportanteId);   

              const valorTipo = this.selectTiposAportante.find( a => a.dominioId == apor.tipoAportanteId);
              const valorNombre = this.nombresAportante.find( a => a.dominioId == apor.nombreAportanteId);

              this.commonService.listaMunicipiosByIdDepartamento(apor.municipioId.toString().substring(0,5)).subscribe( mun => {

                const valorMunicipio = mun.find( a => a.localizacionId == apor.municipioId.toString());
                const valorDepartamento = this.departamentos.find( a => a.localizacionId == apor.municipioId.toString().substring(0,5));

                grupo.get('departamento').setValue(valorDepartamento);
                grupo.get('municipios').setValue(mun);
                grupo.get('municipio').setValue(valorMunicipio);
                grupo.get('tipo').setValue(valorTipo);
                grupo.get('nombre').setValue(valorNombre);

                this.aportantes.push( grupo );
              })

            });
  
            this.listaCofinancAportantes = cof.cofinanciacionAportante;
            this.actualizarValores();

           });
       }
    });
  }

  actualizarValores(){
      let valorTotal: number = 0;
      let valorAportante: number = 0;
      this.listaCofinancAportantes.forEach(apo =>{
         apo.cofinanciacionDocumento.forEach( doc => {
            valorTotal += doc.valorDocumento?parseInt(doc.valorDocumento):0;
          })
      });

      this.valorTotalAcuerdo = valorTotal;
  }

  ngOnInit(): void {

    this.vigenciasAportante = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    this.vigenciaEstados = this.cofinanciacionService.vigenciasAcuerdoCofinanciacion();
    
    forkJoin
    ([
      this.commonService.listaTipoDocFinanciacion(),
      this.commonService.listaTipoAportante(),
      this.commonService.listaNombreAportante(),
      this.commonService.listaDepartamentos(),
    ]).subscribe
      (
        res =>
        {
          this.tiposDocumento = res[0];
          this.selectTiposAportante = res[1];
          this.nombresAportante = res[2];
          this.departamentos = res[3];
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
  
  changeDepartamento(id){
    this.commonService.listaMunicipiosByIdDepartamento(this.aportantes.controls[id].get('departamento').value.localizacionId).subscribe( mun =>
      {
        this.aportantes.controls[id].get('municipios').setValue(mun);
      });
  }

  changeTipoAportante(p){
    console.log(p);
  }

  CambioNumeroAportantes(){
    const FormNumAportantes = this.datosAportantes.value;
    if (FormNumAportantes.numAportes > this.aportantes.length && FormNumAportantes.numAportes < 100) {
      while (this.aportantes.length < FormNumAportantes.numAportes) {
        this.aportantes.push( this.createAportante() );
      }
    } else if (FormNumAportantes.numAportes <= this.aportantes.length && FormNumAportantes.numAportes >= 0) {
      while (this.aportantes.length > FormNumAportantes.numAportes) {
        this.borrarAportante(this.aportantes, this.aportantes.length - 1);
        //this.listaCofinancAportantes.pop();
      }
    }
  }

  createAportante(): FormGroup {
    
    let grupo: FormGroup = this.fb.group({
      tipo: ['', Validators.required],
      nombre: [''],
      cauntosDocumentos: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
      cofinanciacionId:[''],
      cofinanciacionAportanteId:[''],
      departamento: [''],
      municipio: [''],
      municipios: ['']
    });


    let cofiApo: CofinanciacionAportante={
      tipoAportanteId:  '',
      nombreAportanteId:'',
      municipioId:      0,
      cofinanciacionId: 0,
      cofinanciacionAportanteId:0,
      cofinanciacionDocumento:[]
    }
    this.listaCofinancAportantes.push(cofiApo);

    return grupo

  }

  createAportanteEditar(pTipo: number, pNombre: number, pCantidad: number, pCofinanciacionId: number, pCofinanciacionAportanteId: number): FormGroup {
    let grupo: FormGroup = this.fb.group({
      tipo: [pTipo, Validators.required],
      nombre: [pNombre],
      cauntosDocumentos: [pCantidad, [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
      cofinanciacionId: [pCofinanciacionId],
      cofinanciacionAportanteId: [pCofinanciacionAportanteId],
      departamento: [''],
      municipio: [''],
      municipios: ['']
    });

    for (let i=0; i<pCantidad; i++)
    {
      let cofiApo: CofinanciacionAportante={
        tipoAportanteId:  '',
        nombreAportanteId:'',
        municipioId:      0,
        cofinanciacionId: 0,
        cofinanciacionAportanteId:0,
        cofinanciacionDocumento:[]
      }
      this.listaCofinancAportantes.push(cofiApo);
    }

    return grupo;

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
    const index = this.listaCofinancAportantes.indexOf(this.listaCofinancAportantes[i]);
    this.listaCofinancAportantes.splice(index, 1);
  }

  listaAportantes()
  {
    let listaAportantesTemp = [];
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
          tipoAportanteId:  control.get('tipo').value?      control.get('tipo').value.dominioId:null,
          nombreAportanteId:control.get('nombre').value?    control.get('nombre').value.dominioId:null,
          municipioId:      control.get('municipio').value? control.get('municipio').value.localizacionId:null,
          cofinanciacionId: control.get('cofinanciacionId').value,
          cofinanciacionAportanteId:control.get('cofinanciacionAportanteId').value,
          cofinanciacionDocumento:cofinanciacionDocumento
        }

        listaAportantesTemp.push(cofiApo);
        //this.listaCofinancAportantes.push(cofiApo);
        i++;
      });    
    this.listaCofinancAportantes=listaAportantesTemp;  
      //this.mostrarDocumentosDeApropiacion = true;
  }
  
  getAportantes(){
    return 
  }

  onSubmit() {
    console.log("entró");
    if (this.datosAportantes.valid) {
      //this.listaCofinancAportantes = [];
      this.listaAportantes();
      let cofinanciacion: Cofinanciacion = 
      {
        vigenciaCofinanciacionId: this.datosAportantes.get('vigenciaEstado').value,
        cofinanciacionAportante: this.listaCofinancAportantes,
        cofinanciacionId: this.id
      }

      console.log(cofinanciacion);

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
    let cantidadDocumentos: number = data.get('cauntosDocumentos').value;
    console.log(cantidadDocumentos, ' ', this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length)
    if (cantidadDocumentos > this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length 
          && cantidadDocumentos < 100) {
            console.log('asdas')
      while (this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length < cantidadDocumentos) {
    
        this.listaCofinancAportantes[identificador].cofinanciacionDocumento.push(
          {
            cofinanciacionAportanteId:identificador,
            fechaAcuerdo:null,
            cofinanciacionDocumentoId:null,
            numeroActa:null,
            tipoDocumentoId:null,
            valorDocumento:null,
            valorTotalAportante:null,
            vigenciaAporte:null
          });
      
      }
    } else if (cantidadDocumentos <= this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length 
          && cantidadDocumentos >= 0) {
      while (this.listaCofinancAportantes[identificador].cofinanciacionDocumento.length > cantidadDocumentos) {
        this.listaCofinancAportantes[identificador].cofinanciacionDocumento.pop();
      }
    }

    
    

  }
}
