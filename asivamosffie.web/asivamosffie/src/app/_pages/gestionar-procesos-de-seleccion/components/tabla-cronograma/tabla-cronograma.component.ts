import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { ProcesoSeleccionService, ProcesoSeleccionCronograma, ProcesoSeleccionMonitoreo, ProcesoSeleccionCronogramaMonitoreo } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute } from '@angular/router';
import { from } from 'rxjs';
import { mergeMap, tap, toArray } from 'rxjs/operators';
import { CommonService, Dominio, Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EstadosSolicitud, EstadosSolicitudCronograma } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-tabla-cronograma',
  templateUrl: './tabla-cronograma.component.html',
  styleUrls: ['./tabla-cronograma.component.scss']
})
export class TablaCronogramaComponent implements OnInit {

  @Input() editMode: any = {};

  bitEditar=true;

  addressForm = this.fb.array([]);
  maxDate: Date;
  listaCronograma: ProcesoSeleccionCronograma[] = [];
  idProcesoSeleccion: number = 0;
  listaetapaActualProceso: Dominio[]=[];

  editorStyle = {
    height: '100px',
    width: '600px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };



  constructor(
    private fb: FormBuilder,
    private procesoSeleccionService: ProcesoSeleccionService,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog,
    private commonService:CommonService

  ) {
    this.maxDate = new Date();
  }


  ngOnInit(): void {

    this.commonService.listaEtapaActualProceso().subscribe(result=>{
      this.listaetapaActualProceso=result; 
    });
    this.activatedRoute.params.subscribe(parametro => {
      this.idProcesoSeleccion = parametro['id'];

      this.procesoSeleccionService.listaProcesoSeleccionCronogramaMonitoreo(this.idProcesoSeleccion).subscribe(monitoreo => {
        if(monitoreo.length==0)
        {
          this.procesoSeleccionService.listaActividadesByIdProcesoSeleccion(this.idProcesoSeleccion).subscribe(lista => {

            let listaActividades = this.addressForm as FormArray;
            this.listaCronograma = lista;            
    
            lista.forEach(cronograma => {
              let grupo = this.crearActividad();
              const etapaActualproceso = this.listaetapaActualProceso.find(p => p.codigo === cronograma.etapaActualProcesoCodigo);
              grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
              grupo.get('descripcion').setValue(cronograma.descripcion);
              grupo.get('fecha').setValue(cronograma.fechaMaxima);
              grupo.get('etapaActualProceso').setValue(etapaActualproceso),
              listaActividades.push(grupo);
    
            })
    
          })
        }
        else{
          let listaActividades = this.addressForm as FormArray;
          this.listaCronograma = monitoreo[monitoreo.length-1].procesoSeleccionCronogramaMonitoreo;                
          if(this.listaCronograma)
          {
            
            this.listaCronograma.forEach(cronograma => {
              let grupo = this.crearActividad();
              const etapaActualproceso = this.listaetapaActualProceso.find(p => p.codigo === cronograma.etapaActualProcesoCodigo);
              grupo.get('procesoSeleccionCronogramaId').setValue(cronograma.procesoSeleccionCronogramaId);
              grupo.get('descripcion').setValue(cronograma.descripcion);
              grupo.get('fecha').setValue(cronograma.fechaMaxima);
              grupo.get('etapaActualProceso').setValue(etapaActualproceso),    
              listaActividades.push(grupo);    
              if(cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.Creada && cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteFiduciario &&
                cronograma.estadoActividadCodigo!=EstadosSolicitudCronograma.DevueltaPorComiteTecnico)
                {
                  this.bitEditar=false;
                }
            })
          }            
        }
      });
      

    })
  }

  agregaFuente() {
    this.addressForm.push(this.crearActividad());
  }

  crearActividad(): FormGroup {
    return this.fb.group({
      procesoSeleccionCronogramaId: [],
      descripcion: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(500)
      ])],
      fecha: [null, Validators.required],
      etapaActualProceso: [null, Validators.required],
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {

    let listaActividades = this.addressForm as FormArray;
    let listaCronograma:ProcesoSeleccionMonitoreo={estadoActividadCodigo:null,
      numeroProceso:null,
      procesoSeleccionCronogramaMonitoreo:[],
      procesoSeleccionId:null, procesoSeleccionMonitoreoId:null
    };

    let i = 0;
    listaActividades.controls.forEach(control => {
      let procesoSeleccionCronograma: ProcesoSeleccionCronogramaMonitoreo = {
        procesoSeleccionCronogramaId: control.get('procesoSeleccionCronogramaId').value,
        descripcion: control.get('descripcion').value,
        fechaMaxima: control.get('fecha').value,
        etapaActualProcesoCodigo: control.get('etapaActualProceso').value?control.get('etapaActualProceso').value.codigo:null,
        //procesoSeleccionId: this.idProcesoSeleccion,
        numeroActividad: i,

      }
      listaCronograma.procesoSeleccionCronogramaMonitoreo.push(procesoSeleccionCronograma);
      listaCronograma.procesoSeleccionId=this.idProcesoSeleccion;
      i++
    })
    this.procesoSeleccionService.createEditarProcesoSeleccionCronogramaMonitoreo(listaCronograma).subscribe(respuesta => {
     
      if (respuesta.code == "200")
        this.openDialog("", respuesta.message);
    });

    /*from(this.listaCronograma)
      .pipe(mergeMap(cronograma => this.procesoSeleccionService.createEditarProcesoSeleccionCronogramaMonitoreo(cronograma)
        .pipe(
          tap()
        )
      ),
        toArray())
      .subscribe(respuesta => {
        let res = respuesta[0] as Respuesta
        if (res.code == "200")
          this.openDialog("", res.message);
        console.log(respuesta);

        //jflorez deshabilito el modo visualización
        //this.editMode.valor = false; 
      })
    */
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      //if (result) {
        location.reload();
      //}
    });


  }

}
