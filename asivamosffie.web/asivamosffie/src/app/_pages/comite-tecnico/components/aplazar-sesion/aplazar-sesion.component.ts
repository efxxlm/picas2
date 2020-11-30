import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { ComiteTecnicoComponent } from '../comite-tecnico/comite-tecnico.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-aplazar-sesion',
  templateUrl: './aplazar-sesion.component.html',
  styleUrls: ['./aplazar-sesion.component.scss']
})
export class AplazarSesionComponent implements OnInit {

  fechaAplazamiento: FormControl;
  minDate: Date;

  constructor(
                public dialogRef: MatDialogRef<AplazarSesionComponent>, 
                @Inject(MAT_DIALOG_DATA) public data: { 
                                            comite: ComiteTecnico, 
                                            //objetoComiteTecnico: ComiteTecnico 
                                          },
                private technicalCommitteeSessionService: TechnicalCommitteSessionService,
                public dialog: MatDialog,
                private router: Router,
                
             ) 
  {
    this.declararFechaAplazamiento();
    this.minDate = new Date();
  }

  ngOnInit(): void {
  }

  private declararFechaAplazamiento() {
    this.fechaAplazamiento = new FormControl(null, [Validators.required]);
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit(){

    let comiteTecnico: ComiteTecnico = {
      comiteTecnicoId: this.data.comite.comiteTecnicoId,
      fechaAplazamiento: this.fechaAplazamiento.value
    }

    this.technicalCommitteeSessionService.aplazarSesionComite( comiteTecnico )
      .subscribe( respuesta => {
        this.openDialog( '', `<b>${respuesta.message}</b>` )
        if ( respuesta.code == "202" )
        {
          this.dialogRef.close();
          this.router.navigate(['/comiteTecnico']);
        }
      })
    
  }

}
