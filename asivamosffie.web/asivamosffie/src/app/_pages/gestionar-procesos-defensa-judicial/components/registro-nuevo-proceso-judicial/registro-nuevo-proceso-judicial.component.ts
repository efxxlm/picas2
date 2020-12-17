import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-registro-nuevo-proceso-judicial',
  templateUrl: './registro-nuevo-proceso-judicial.component.html',
  styleUrls: ['./registro-nuevo-proceso-judicial.component.scss']
})
export class RegistroNuevoProcesoJudicialComponent implements OnInit {
  addressForm = this.fb.group({
    legitimacionActiva: [null, Validators.required],
    tipoProceso: [null, Validators.required],
  });
  tipoProcesoArray = [];
  constructor(private fb: FormBuilder, public dialog: MatDialog, public commonServices: CommonService) { }

  ngOnInit(): void {
    this.commonServices.listaProcesosJudiciales().subscribe(
      response=>{
        this.tipoProcesoArray=response;
      }
    );

  }

}
