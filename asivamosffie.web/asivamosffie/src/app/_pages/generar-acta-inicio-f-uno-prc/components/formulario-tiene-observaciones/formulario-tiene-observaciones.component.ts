import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';

@Component({
  selector: 'app-formulario-tiene-observaciones',
  templateUrl: './formulario-tiene-observaciones.component.html',
  styleUrls: ['./formulario-tiene-observaciones.component.scss']
})
export class FormularioTieneObservacionesComponent implements OnInit {
  addressForm = this.fb.group({});

  @Input() contratoId;
  dataElements: any;
  tieneObservacionesBool: boolean;
  observacionesUltimas: any;
  contratoObservacionId: any;
  constructor(private router: Router,public dialog: MatDialog, private fb: FormBuilder, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.loadService(this.contratoId);
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });   
  }
  editorStyle = {
    height: '100px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: ['', Validators.required],
      observaciones:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }
  loadService(id){
    this.service.GetListContratoObservacionByContratoId(id).subscribe(data=>{
      this.dataElements = data;
      for(let i=0; i<this.dataElements.length;i++){ 
        this.tieneObservacionesBool = this.dataElements[i].esActaFase1;
        this.observacionesUltimas = this.dataElements[i].observaciones;
        this.contratoObservacionId = this.dataElements[i].contratoObservacionId;
      }
      if(localStorage.getItem("editable")=="true"){
        this.addressForm.get('tieneObservaciones').setValue(this.tieneObservacionesBool);
        this.addressForm.get('observaciones').setValue(this.observacionesUltimas);
      }
    });
  }
  generarActaSuscrita(){
    this.service.GetActaByIdPerfil(8,this.contratoId).subscribe(resp=>{
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
  onSubmit() {
    if(localStorage.getItem("editable")=="false"){
      const contratoObservacion={
        'ContratoId':this.contratoId,
        'Observaciones':this.addressForm.value.observaciones,
        'EsActa':true,
        'EsActaFase1':this.addressForm.value.tieneObservaciones
      };
      this.service.CreateEditObservacionesActa(contratoObservacion).subscribe(data=>{
        if(data.code=="200"){
          if(this.addressForm.value.tieneObservaciones==true){
            if(localStorage.getItem("origin")=="interventoria"){
              this.service.CambiarEstadoActa(this.contratoId,"8").subscribe(data0=>{
            
              });
            }
            else{
              this.service.CambiarEstadoActa(this.contratoId,"16").subscribe(data0=>{
            
              });
            }
          }
          else{
            if(localStorage.getItem("origin")=="interventoria"){
              this.service.CambiarEstadoActa(this.contratoId,"5").subscribe(data1=>{
            
              });
            }
            else{
              this.service.CambiarEstadoActa(this.contratoId,"15").subscribe(data1=>{
            
              });
            }
          }
          this.openDialog('La información ha sido guardada exitosamente.', "");
          this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
        }
        else{
          this.openDialog(data.message, "");
        }
      });
    }
    else{
      const contratoObservacion={
        'ContratoObservacionId':this.contratoObservacionId,
        'ContratoId':this.contratoId,
        'Observaciones':this.addressForm.value.observaciones,
        'EsActa':true,
        'EsActaFase1':this.addressForm.value.tieneObservaciones
      };
      this.service.CreateEditObservacionesActa(contratoObservacion).subscribe(data2=>{
        if(data2.code=="200"){
          if(this.addressForm.value.tieneObservaciones==true){
            if(localStorage.getItem("origin")=="interventoria"){
              this.service.CambiarEstadoActa(this.contratoId,"8").subscribe(data0=>{
            
              });
            }
            else{
              this.service.CambiarEstadoActa(this.contratoId,"16").subscribe(data0=>{
            
              });
            }
          }
          else{
            if(localStorage.getItem("origin")=="interventoria"){
              this.service.CambiarEstadoActa(this.contratoId,"5").subscribe(data1=>{
            
              });
            }
            else{
              this.service.CambiarEstadoActa(this.contratoId,"15").subscribe(data1=>{
            
              });
            }
          }
          this.openDialog('La información ha sido guardada exitosamente.', "");
          this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
        }
        else{
          this.openDialog(data2.message, "");
        }

      })
    }
    console.log(this.addressForm.value);
  }
}
