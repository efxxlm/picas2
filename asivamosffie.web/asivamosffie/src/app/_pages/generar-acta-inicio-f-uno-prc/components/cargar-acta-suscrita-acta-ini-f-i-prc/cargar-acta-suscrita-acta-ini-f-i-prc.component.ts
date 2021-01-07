import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';

@Component({
  selector: 'app-cargar-acta-suscrita-acta-ini-f-i-prc',
  templateUrl: './cargar-acta-suscrita-acta-ini-f-i-prc.component.html',
  styleUrls: ['./cargar-acta-suscrita-acta-ini-f-i-prc.component.scss']
})
export class CargarActaSuscritaActaIniFIPreconstruccionComponent implements OnInit {
  addressForm = new FormGroup({
    fechaFirmaContratistaObra: new FormControl(),
    fechaFirmaContratistaInterventoria: new FormControl(),
    documentoFile: new FormControl()
  })

  boton: string = "Cargar";
  archivo: string;
  maxDate: Date;
  maxDate2: Date;


  public idContrato;
  public idRol;
  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public estadoDocumentoCodigo;
  public fechaEnvioFirma;
  public fechaFirmaFiduciaria;
  public numContrato;
  public fechaFirmaContrato;
  public fechaActaInicioFase1;
  public fechaTerminacion;
  public plazoFase1PreMeses;
  public plazoFase1PreDias;
  public plazoFase2ConstruccionMeses;
  public plazoFase2ConstruccionDias;
  public observaciones;
  public rutaDocumento;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  public fechaFirmaContratistaObra;
  public fechaFirmaContratistaInterventoria;

  public fecha1Titulo;
  public fecha2Titulo;

  prueba: any;
  esRojo: boolean = false;

  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, public matDialogRef: MatDialogRef<CargarActaSuscritaActaIniFIPreconstruccionComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private service: GestionarActPreConstrFUnoService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
    if (data.id != undefined) {
      this.idContrato = data.id;
    }
    if (data.idRol != undefined) {
      this.idRol = data.idRol;
    }
    if (data.numContrato != undefined) {
      this.numContrato = data.numContrato;
    }
    if (data.fecha1Titulo != undefined) {
      this.fecha1Titulo = data.fecha1Titulo;
    }
    if (data.fecha2Titulo != undefined) {
      this.fecha2Titulo = data.fecha2Titulo;
    }
  }
  ngOnInit(): void {
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  fileName(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.addressForm.patchValue({
        documentoFile: file
      });
    }
  }
  onSubmit() {
    const pContrato = new FormData();
    this.fechaSesion = new Date(this.addressForm.value.fechaFirmaContratistaObra);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;
    this.fechaSesion2 = new Date(this.addressForm.value.fechaFirmaContratistaInterventoria);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`;
    let pFile = this.addressForm.get('documentoFile').value;
    pFile = pFile.name.split('.');
    pFile = pFile[pFile.length - 1];
    if (pFile === 'pdf') {
      pContrato.append('ContratoId', this.idContrato);
      pContrato.append('FechaActaInicioFase1', this.fechaSesionString);
      pContrato.append('FechaTerminacion', this.fechaSesionString2);
      pContrato.append('pFile', this.addressForm.get('documentoFile').value);
      if (this.fechaSesionString == 'NaN-NaN-NaN' || this.fechaSesionString2 == 'NaN-NaN-NaN' || this.archivo == undefined) {
        this.openDialog('', '<b>Falta registrar información.</b>');
        this.esRojo = true;
      }
      else {
        this.service.LoadActa(pContrato).subscribe((data: any) => {

          if (data.code == "200") {
            this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
            this.close();
          }
          else {
            this.openDialog("", data.message);
          }
        });
      }
    } else {
      this.openDialog('', '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .pdf</b>');
      return;
    }



  }
  close() {
    this.matDialogRef.close('aceptado');
  }
}
